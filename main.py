import matplotlib.pyplot as plt

# Points provided
points_str = """-3138 -2512
6804 -1072
-193 8782
-5168 2636
-8022 -3864
-9955 -2923
-7005 2118
7775 -8002
4244 -1339
9478 -1973
-7795 -5000
-4521 1266
-192 3337
-9860 1311"""

# Convert points to list of tuples
points = [tuple(map(int, point.split())) for point in points_str.split('\n')]

# Define the order to connect points
order = [0, 1, 7, 9, 8, 12, 3, 6, 11, 13, 5, 4, 10, 2, 0]

# Extract x and y coordinates separately
x_coords = [point[0] for point in points]
y_coords = [point[1] for point in points]

# Plot points
plt.scatter(x_coords, y_coords)

# Plot lines
for i in range(len(order) - 1):
    plt.plot([points[order[i]][0], points[order[i+1]][0]], [points[order[i]][1], points[order[i+1]][1]], 'k-')

# Connect the last point to the first
plt.plot([points[order[-1]][0], points[order[0]][0]], [points[order[-1]][1], points[order[0]][1]], 'k-')

# Show plot
plt.show()
