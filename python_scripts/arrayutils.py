# Given a matrix, return maximum value and "location x,y" of the maximum value
def get_matrix_max_value_index(matrix):
    maxValue = matrix[0][0]
    maxi = 0
    maxj = 0
    for i in range(len(matrix)):
        for j in range(len(matrix[i])):
            if (matrix[i][j] > maxValue):
                maxValue = matrix[i][j]
                maxi = i
                maxj = j
    return maxValue, maxi, maxj