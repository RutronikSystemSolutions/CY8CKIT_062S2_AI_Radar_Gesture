import numpy as np
import json
import radarprocessor
import matplotlib.pyplot as plt
import arrayutils
import anglediff

print('Python script enabling to extract magnitude and to compute angle of arrival')

# --------------------------------------------------------------------------------------
# Load radar data and configuration data
file_path = ("data\\session_120260420-155131\\")

radar_data = np.load(file_path + "RadarIfxAvian_00\\radar.npy")

configuration_file = open(file_path + "RadarIfxAvian_00\\config.json")
radar_configuration = json.load(configuration_file)
configuration_file.close()

# --------------------------------------------------------------------------------------
# Extract some important parameters
sampling_frequency = int(
    radar_configuration
    ['device_config']['fmcw_single_shape']['sample_rate_Hz'])

start_frequency = int(
    radar_configuration
    ['device_config']['fmcw_single_shape']['start_frequency_Hz'])

end_frequency = int(
    radar_configuration
    ['device_config']['fmcw_single_shape']['end_frequency_Hz'])

samples_per_chirp = int(
    radar_configuration
    ['device_config']['fmcw_single_shape']['num_samples_per_chirp'])

chirps_per_frame = int(
    radar_configuration
    ['device_config']['fmcw_single_shape']['num_chirps_per_frame'])

celerity = 299792458

fft_len = int((samples_per_chirp / 2) + 1)

slope = (end_frequency - start_frequency) / (samples_per_chirp * (1. / sampling_frequency))
maximum_range = ((sampling_frequency / 2.)  * celerity) / (2. * slope)
print("Maximum range: " + str(maximum_range) + " m")
print("FFT len: " + str(fft_len))

# Enough RX antennas?
if (len(radar_configuration['device_config']['fmcw_single_shape']['rx_antennas']) < 2):
    print("Not enough RX antennas to compute angle of arrival, exiting")
    exit()

# --------------------------------------------------------------------------------------
# Init radar processor and set data and configuration
radar_processor = radarprocessor.RadarProcessor()
radar_processor.set_data_and_config(samples_per_chirp=samples_per_chirp,
                                    chirps_per_frame=chirps_per_frame,
                                    radar_data=radar_data,
                                    fft_len=fft_len)

# --------------------------------------------------------------------------------------
# As example, compute Range FFT for RX antenna index 0 and frame index as selected and plot it
selected_frame_index = 49
range_fft_matrix_example = radar_processor.get_frame_range_fft_matrix(frame_index=selected_frame_index, rx_index=0)
plt.figure()
plt.imshow(20 * np.log10(np.abs(range_fft_matrix_example)), aspect='auto',
           extent=[0, maximum_range, chirps_per_frame, 0])
plt.title("Range FFT for RX2, and frame " + str(selected_frame_index))
plt.grid(True)
plt.xlabel("Range (m)")
plt.ylabel("Chirp index")

# --------------------------------------------------------------------------------------
# Compute the Doppler FFT using the previous range FFT matrix and plot it
doppler_fft_matrix_example = radar_processor.get_doppler_fft_matrix(range_fft_matrix_example)
plt.figure()
plt.imshow(20 * np.log10(np.abs(doppler_fft_matrix_example)), aspect='auto',
           origin='lower',
           extent=[-sampling_frequency / 2, sampling_frequency / 2, 0, maximum_range])
plt.title("Doppler FFT for RX2, frame "+ str(selected_frame_index))
plt.grid(True)
plt.xlabel("Doppler frequency (Hz)")
plt.ylabel("Range (m)")

# --------------------------------------------------------------------------------------
# Loop through the frame and compute Doppler FFT
# if maximum peak bigger than threshold, compute doppler FFT for the second antenna 
# and then compute angle of arrival

magnitude_over_time = []
aoa_over_time = []
threshold = 1

x_over_time = []
y_over_time = []
z_over_time = []

for frame_index in range(radar_data.shape[0]):
    range_fft_matrix_rx2 = radar_processor.get_frame_range_fft_matrix(frame_index=frame_index, rx_index=0)
    doppler_fft_matrix_rx2 = radar_processor.get_doppler_fft_matrix(range_fft_matrix_rx2)

    # Compute the biggest magnitude
    maxValue, maxRange, maxVelocity = arrayutils.get_matrix_max_value_index(np.abs(doppler_fft_matrix_rx2))

    # Store for display
    magnitude_over_time.append(maxValue)

    if (maxValue > threshold):
        range_fft_matrix_rx3 = radar_processor.get_frame_range_fft_matrix(frame_index=frame_index, rx_index=1)
        doppler_fft_matrix_rx3 = radar_processor.get_doppler_fft_matrix(range_fft_matrix_rx3)

        phase_rx2 = np.angle(doppler_fft_matrix_rx2[maxRange, maxVelocity])
        phase_rx3 = np.angle(doppler_fft_matrix_rx3[maxRange, maxVelocity])

        angle_delta = anglediff.get_diff(phase_rx2, phase_rx3)

        # Convert to real angle value
        LAMBDA_VAL = 0.0049 # wavelength for 60 GHz
        ANTENNA_DISTANCE = 0.0025 # distance between the 2 antennas (2.5mm)

        delta_rad = np.sinh(LAMBDA_VAL * angle_delta / (2 * np.pi * ANTENNA_DISTANCE))

        aoa_over_time.append(delta_rad)
        x_over_time.append(np.cos(delta_rad) * (maxRange * maximum_range / fft_len))
        y_over_time.append(np.sin(delta_rad) * (maxRange * maximum_range / fft_len))
        z_over_time.append(frame_index)
    else:
        aoa_over_time.append(0)

plt.figure()
plt.plot(magnitude_over_time)
plt.title("Magnitude of the biggest peak over time")
plt.grid(True)

plt.figure()
plt.plot(aoa_over_time)
plt.title("Angle of arrival over time")
plt.grid(True)

plt.figure()
plt.scatter(x_over_time, y_over_time, c=z_over_time, cmap='viridis')
plt.title("Position of the target over time")
plt.xlabel("x (m)")
plt.ylabel("y (m)")
plt.colorbar(label='Frame index')
plt.grid(True)

plt.show()