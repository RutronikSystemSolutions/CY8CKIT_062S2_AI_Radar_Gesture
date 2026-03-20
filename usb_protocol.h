/******************************************************************************
* File Name:   usb_protocol.h
*
*  Created on: 2026-03-17
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

#ifndef USB_PROTOCOL_H_
#define USB_PROTOCOL_H_

#include <stdint.h>

#define USB_PROTOCOL_COMMAND_START		49
#define USB_PROTOCOL_COMMAND_STOP		50
#define USB_PROTOCOL_COMMAND_GET_CONFIG	51

#define USB_PROTOCOL_RADAR_CONFIG_SIZE	33

/**
 * @brief Generate communication buffer for configuration
 *
 * The size of the buffer should be at least: 33 bytes
 */
void usb_protocol_generate_radar_configuration(uint64_t start_freq,
		uint64_t end_freq,
		uint16_t samples_per_chirp,
		uint16_t chirps_per_frame,
		uint8_t rx_antennas,
		uint32_t sample_rate,
		float chirp_repetition_time_s,
		float frame_repetition_time_s,
		uint8_t* buffer);

#endif /* USB_PROTOCOL_H_ */
