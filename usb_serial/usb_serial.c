/******************************************************************************
* File Name:   usb_serial.c
*
*  Created on: 2026-02-05
*  Company: Rutronik Elektronische Bauelemente GmbH
*  Author: ROJ030, GDR
*
*******************************************************************************
* (c) 2019-2021, Cypress Semiconductor Corporation. All rights reserved.
*******************************************************************************
* This software, including source code, documentation and related materials
* ("Software"), is owned by Cypress Semiconductor Corporation or one of its
* subsidiaries ("Cypress") and is protected by and subject to worldwide patent
* protection (United States and foreign), United States copyright laws and
* international treaty provisions. Therefore, you may use this Software only
* as provided in the license agreement accompanying the software package from
* which you obtained this Software ("EULA").
*
* If no EULA applies, Cypress hereby grants you a personal, non-exclusive,
* non-transferable license to copy, modify, and compile the Software source
* code solely for use in connection with Cypress's integrated circuit products.
* Any reproduction, modification, translation, compilation, or representation
* of this Software except as specified above is prohibited without the express
* written permission of Cypress.
*
* Disclaimer: THIS SOFTWARE IS PROVIDED AS-IS, WITH NO WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, NONINFRINGEMENT, IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. Cypress
* reserves the right to make changes to the Software without notice. Cypress
* does not assume any liability arising out of the application or use of the
* Software or any product or circuit described in the Software. Cypress does
* not authorize its products for use in any products where a malfunction or
* failure of the Cypress product may reasonably be expected to result in
* significant property damage, injury or death ("High Risk Product"). By
* including Cypress's product in a High Risk Product, the manufacturer of such
* system or application assumes all risk of such use and in doing so agrees to
* indemnify Cypress against all liability.
*
* Rutronik Elektronische Bauelemente GmbH Disclaimer: The evaluation board
* including the software is for testing purposes only and,
* because it has limited functions and limited resilience, is not suitable
* for permanent use under real conditions. If the evaluation board is
* nevertheless used under real conditions, this is done at one’s responsibility;
* any liability of Rutronik is insofar excluded
*******************************************************************************/

#include "usb_serial.h"
#include "cycfg.h"

#include "hal_timer.h"

#define USB_SERIAL_PORT    	(0U)

/*******************************************************************************
* Function Prototypes
********************************************************************************/
static void usb_high_isr(void);
static void usb_medium_isr(void);
static void usb_low_isr(void);

/*******************************************************************************
* Global Variables
********************************************************************************/
/* USB Interrupt Configuration */
const cy_stc_sysint_t usb_high_interrupt_cfg =
{
    .intrSrc = (IRQn_Type) usb_interrupt_hi_IRQn,
    .intrPriority = 5U,
};
const cy_stc_sysint_t usb_medium_interrupt_cfg =
{
    .intrSrc = (IRQn_Type) usb_interrupt_med_IRQn,
    .intrPriority = 6U,
};
const cy_stc_sysint_t usb_low_interrupt_cfg =
{
    .intrSrc = (IRQn_Type) usb_interrupt_lo_IRQn,
    .intrPriority = 7U,
};

/* USBDEV context variables */
cy_stc_usbfs_dev_drv_context_t  usb_drvContext;
cy_stc_usb_dev_context_t        usb_devContext;
cy_stc_usb_dev_cdc_context_t    usb_cdcContext;


_Bool usb_serial_init(void)
{
    /* Initialize the USB device */
    Cy_USB_Dev_Init(USB_DEV_HW, &USB_DEV_config, &usb_drvContext,&usb_devices[0], &usb_devConfig, &usb_devContext);

    /* Initialize the CDC Class */
    Cy_USB_Dev_CDC_Init(&usb_cdcConfig, &usb_cdcContext, &usb_devContext);

    /* Initialize the USB interrupts */
    Cy_SysInt_Init(&usb_high_interrupt_cfg,   &usb_high_isr);
    Cy_SysInt_Init(&usb_medium_interrupt_cfg, &usb_medium_isr);
    Cy_SysInt_Init(&usb_low_interrupt_cfg,    &usb_low_isr);

    /* Enable the USB interrupts */
    NVIC_EnableIRQ(usb_high_interrupt_cfg.intrSrc);
    NVIC_EnableIRQ(usb_medium_interrupt_cfg.intrSrc);
    NVIC_EnableIRQ(usb_low_interrupt_cfg.intrSrc);

    /* Make device appear on the bus. This function call is blocking,
       it waits till the device enumerates */
    Cy_USB_Dev_Connect(true, CY_USB_DEV_WAIT_FOREVER, &usb_devContext);

    // Init timer (used for timeout detection)
    if (hal_timer_init() != 0) return false;

   // TODO: check if everything gets initialized correctly

    return true;
}


/***************************************************************************
* Function Name: usb_high_isr
********************************************************************************
* Summary:
*  This function processes the high priority USB interrupts.
*
***************************************************************************/
static void usb_high_isr(void)
{
    /* Call interrupt processing */
    Cy_USBFS_Dev_Drv_Interrupt(USB_DEV_HW, Cy_USBFS_Dev_Drv_GetInterruptCauseHi(USB_DEV_HW),
                               &usb_drvContext);
}


/***************************************************************************
* Function Name: usb_medium_isr
********************************************************************************
* Summary:
*  This function processes the medium priority USB interrupts.
*
***************************************************************************/
static void usb_medium_isr(void)
{
    /* Call interrupt processing */
    Cy_USBFS_Dev_Drv_Interrupt(USB_DEV_HW, Cy_USBFS_Dev_Drv_GetInterruptCauseMed(USB_DEV_HW),
                               &usb_drvContext);
}


/***************************************************************************
* Function Name: usb_low_isr
********************************************************************************
* Summary:
*  This function processes the low priority USB interrupts.
*
**************************************************************************/
static void usb_low_isr(void)
{
    /* Call interrupt processing */
    Cy_USBFS_Dev_Drv_Interrupt(USB_DEV_HW, Cy_USBFS_Dev_Drv_GetInterruptCauseLo(USB_DEV_HW),
                               &usb_drvContext);
}

int usb_serial_is_data_available()
{
	if (Cy_USB_Dev_CDC_IsDataReady(USB_SERIAL_PORT, &usb_cdcContext))
	{
		return 1;
	}
	return 0;
}

int usb_serial_read(uint8_t* buffer, uint16_t size)
{
	uint32_t read_size = Cy_USB_Dev_CDC_GetData(USB_SERIAL_PORT, buffer, (uint32_t)size, &usb_cdcContext);
	if (read_size != size) return -1;
	return 0;
}

int usb_serial_send(uint8_t* buffer, uint16_t size)
{
	uint8_t* addr = buffer;
	uint16_t remaining = size;

	for(;;)
	{
		// Maximum 64 bytes per sending
		uint16_t count = (remaining > 64)? 64 : remaining;

		uint32_t start_time = hal_timer_get_uticks();

		// Wait until ready (max 50ms)
		while (0u == Cy_USB_Dev_CDC_IsReady(USB_SERIAL_PORT, &usb_cdcContext))
		{
			uint32_t timestamp_us = hal_timer_get_uticks();
			if (hal_timer_timeout(start_time, timestamp_us, 50))
			{
				return -1;
			}
		}

		if (Cy_USB_Dev_CDC_PutData(USB_SERIAL_PORT, addr, count, &usb_cdcContext) != CY_USB_DEV_SUCCESS)
		{
			return -2;
		}

		// Increment pointer addr
		addr += count;
		remaining -= count;
		if (remaining == 0) break;
	}

	return 0;
}
