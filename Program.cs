class TSPSolver
{
    static int[,] distances; // 2D array to store distances between cities
    static int numCities;    // Number of cities
    static int[] bestRoute;  // Best route found so far
    static int minDistance;  // Length of the best route found so far

    static void Main()
    {
        // Example usage:
        // Define the distances between cities (replace with your own data)
        distances = new int[,]
        {
            { 0, 10, 15, 20 },
            { 10, 0, 35, 25 },
            { 15, 35, 0, 30 },
            { 20, 25, 30, 0 }
        };

        numCities = distances.GetLength(0);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Choose solving method:");
            Console.WriteLine("1. Brute Force");
            Console.WriteLine("2. Branch and Bound");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        bestRoute = null;
                        minDistance = int.MaxValue;
                        int[] route = BruteForceSolveTSP();
                        PrintSolution(route);
                        break;
                    case 2:
                        bestRoute = new int[numCities];
                        minDistance = int.MaxValue;
                        int[] routeBB = BranchAndBoundSolveTSP();
                        PrintSolution(routeBB);
                        break;
                    case 3:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer choice.");
            }
        }
    }

    // Brute Force algorithm
    static int[] BruteForceSolveTSP()
    {
        int[] currentRoute = new int[numCities];

        // Initialize current route with the starting city (city 0)
        currentRoute[0] = 0;
        for (int i = 1; i < numCities; i++)
        {
            currentRoute[i] = i;
        }

        // Initialize best route with current route
        bestRoute = new int[numCities];
        Array.Copy(currentRoute, bestRoute, numCities);

        Permute(1, currentRoute);

        return bestRoute;
    }

    // Branch and Bound algorithm
    static int[] BranchAndBoundSolveTSP()
    {
        int[] currentRoute = new int[numCities];

        // Initialize current route with the starting city (city 0)
        currentRoute[0] = 0;
        for (int i = 1; i < numCities; i++)
        {
            currentRoute[i] = i;
        }

        BranchAndBound(currentRoute, 1, 0);

        return bestRoute;
    }

    // Recursive function to generate all permutations
    static void Permute(int k, int[] currentRoute)
    {
        if (k == numCities)
        {
            // Calculate total distance of current route
            int distance = CalculateTotalDistance(currentRoute);

            // Update best route if current route is shorter
            if (distance < minDistance)
            {
                minDistance = distance;
                Array.Copy(currentRoute, bestRoute, numCities);
            }
        }
        else
        {
            for (int i = k; i < numCities; i++)
            {
                // Swap cities at positions k and i
                Swap(ref currentRoute[k], ref currentRoute[i]);

                // Recursively generate permutations for the remaining cities
                Permute(k + 1, currentRoute);

                // Restore the swapped cities to backtrack
                Swap(ref currentRoute[k], ref currentRoute[i]);
            }
        }
    }

    // Branch and Bound algorithm
    static void BranchAndBound(int[] currentRoute, int level, int currentDistance)
    {
        if (level == numCities)
        {
            // Complete route formed, check if it's better than the best route found so far
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                Array.Copy(currentRoute, bestRoute, numCities);
            }
        }
        else
        {
            for (int i = level; i < numCities; i++)
            {
                // Swap cities at positions level and i
                Swap(ref currentRoute[level], ref currentRoute[i]);

                // Calculate lower bound for the partial solution
                int lowerBound = currentDistance + CalculateLowerBound(currentRoute, level);

                // If the lower bound is less than the current best solution, continue branching
                if (lowerBound < minDistance)
                {
                    BranchAndBound(currentRoute, level + 1, currentDistance + distances[currentRoute[level], currentRoute[level - 1]]);
                }

                // Restore the swapped cities to backtrack
                Swap(ref currentRoute[level], ref currentRoute[i]);
            }
        }
    }

    // Function to swap two elements in an array
    static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    // Function to calculate total distance of a route
    static int CalculateTotalDistance(int[] route)
    {
        int totalDistance = 0;
        for (int i = 0; i < numCities - 1; i++)
        {
            totalDistance += distances[route[i], route[i + 1]];
        }
        totalDistance += distances[route[numCities - 1], route[0]]; // Return to the starting city
        return totalDistance;
    }

    // Function to calculate the lower bound of a partial solution
    static int CalculateLowerBound(int[] route, int level)
    {
        int lowerBound = 0;
        for (int i = 0; i < level - 1; i++)
        {
            lowerBound += distances[route[i], route[i + 1]];
        }
        return lowerBound;
    }

    // Function to print the solution
    static void PrintSolution(int[] route)
    {
        Console.WriteLine("Shortest route: " + string.Join(" -> ", route) + " -> 0");
        Console.WriteLine("Total distance: " + CalculateTotalDistance(route));
    }
}