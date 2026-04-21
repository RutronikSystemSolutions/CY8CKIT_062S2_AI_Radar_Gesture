import numpy as np
from scipy import signal

class RadarProcessor:
    def __init__(self):
        self.samples_per_chirp = 0
        self.chirps_per_frame = 0
        self.radar_data = None
        self.fft_len = 0

    def set_data_and_config(self,
                            samples_per_chirp, 
                            chirps_per_frame, 
                            radar_data,
                            fft_len):
        self.samples_per_chirp = samples_per_chirp
        self.chirps_per_frame = chirps_per_frame
        self.radar_data = radar_data
        self.fft_len = fft_len

    # Compute range FFT for an antenna for a frame
    def get_frame_range_fft_matrix(self, frame_index, rx_index):
        range_fft_matrix = np.zeros((self.chirps_per_frame, self.fft_len), dtype=complex)
        window = signal.windows.blackmanharris(self.samples_per_chirp)
        for chirp_index in range(self.chirps_per_frame):
            # Extract time signal
            time_signal = self.radar_data[frame_index, rx_index, chirp_index]

            # Scale between 0 and 1 - remove DC
            time_signal = time_signal / 4095
            time_signal = time_signal - (np.sum(time_signal) / len(time_signal))

            # Apply window
            time_signal = time_signal * window

            range_fft_matrix[chirp_index, :] = np.fft.rfft(time_signal)

        return range_fft_matrix
    
    def get_doppler_fft_matrix(self, range_fft_matrix):
        doppler_fft_matrix = np.zeros((self.fft_len, self.chirps_per_frame), dtype=complex)
        window = signal.windows.blackmanharris(self.chirps_per_frame)
        for range_bin_index in range(self.fft_len):
            # Extract signal for a given bin
            bin_array = range_fft_matrix[:, range_bin_index]

            # Remove DC
            bin_array = bin_array - (np.sum(bin_array) / len(bin_array))

            # Apply window
            bin_array = bin_array * window   

            doppler_fft_matrix[range_bin_index, :] = np.fft.fftshift(np.fft.fft(bin_array))
        return doppler_fft_matrix