import math
import numpy as np

# Get the difference between 2 angles
# Always between -pi and pi
def get_diff(a1, a2):
    sign = -1
    if (a1 > a2):
        sign = 1

    angle = a1 - a2
    k = -sign * math.pi * 2
    if (np.abs(k + angle) < np.abs(angle)):
        return k + angle

    return angle