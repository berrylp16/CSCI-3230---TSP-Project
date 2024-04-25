import matplotlib.pyplot as plt

# Function to parse points from a string
def parse_points(points_str):
    return [tuple(map(int, point.split())) for point in points_str.split('\n')]

# Function to parse order from a string
def parse_order(order_str):
    return list(map(int, order_str.split(' -> ')))

# Function to prompt the user for input with a default value
def prompt_with_default(prompt, default):
    user_input = input(prompt + f" (press Enter to use default: '{default}'):")
    if user_input.strip() == "":
        return default
    return user_input

# Prompt user for filename
filename = input("Enter filename (press Enter to use default string): ")

# Use default string if filename is empty
if not filename:
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
else:
    # Read points from file
    with open(filename, 'r') as file:
        points_str = file.read()

# Parse points
points = parse_points(points_str)

# Prompt user for route
route = prompt_with_default("Enter route", "7 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0 -> 0")

# Parse order
order = parse_order(route)

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
