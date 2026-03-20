/*
 * hal_timer.c
 *
 *  Created on: Feb 12, 2026
 *      Author: jorda
 */

#include "hal_timer.h"
#include "cyhal_timer.h"

static cyhal_timer_t Systick_obj;

int hal_timer_init()
{
	const cyhal_timer_cfg_t Systick_cfg =
	{
		.compare_value = 0,                  // Timer compare value, not used
		.period        = 0xFFFFFFFFUL,              	// Timer period set to a large enough value
		//   compared to event being measured
		.direction     = CYHAL_TIMER_DIR_UP, // Timer counts up
		.is_compare    = false,              // Don't use compare mode
		.is_continuous = false,              // Do not run timer indefinitely
		.value         = 0                   // Initial value of counter
	};


	// Initialize the timer object. Does not use pin output ('pin' is NC) and does not use a
	// pre-configured clock source ('clk' is NULL).
	if (cyhal_timer_init(&Systick_obj, NC, NULL) != CY_RSLT_SUCCESS) return -1;
	if (cyhal_timer_configure(&Systick_obj, &Systick_cfg) != CY_RSLT_SUCCESS) return -2;
	if (cyhal_timer_set_frequency(&Systick_obj, 1000000) != CY_RSLT_SUCCESS) return -3;
	if (cyhal_timer_start(&Systick_obj) != CY_RSLT_SUCCESS) return -4;

	return 0;
}

uint32_t hal_timer_get_uticks(void)
{
	return cyhal_timer_read(&Systick_obj);
}

int hal_timer_timeout(uint32_t start_us, uint32_t now_us, uint32_t timeout_ms)
{
	uint32_t delta_us = 0;
	if (now_us >= start_us)
	{
		// standard, now overflow
		delta_us = now_us - start_us;
	}
	else
	{
		// overflow
		delta_us = 0xffffffffUL - start_us;
		delta_us += now_us;
	}

	uint32_t delta_ms = (delta_us / 1000);
	if (delta_ms > timeout_ms) return 1;
	return 0;
}
