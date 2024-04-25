class TSPSolver
{
    static int[,] distances; // 2D array to store distances between cities
    static int numCities;    // Number of cities
    static int[] bestRoute;  // Best route found so far
    static int minDistance;  // Length of the best route found so far

    static void Main()
    {
        string data = @"-3138 -2512
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
-9860 1311";

        ParseDataAndCalculateDistances(data);

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
                        Stopwatch timerBF = new Stopwatch();
                        timerBF.Start();
                        bestRoute = null;
                        minDistance = int.MaxValue;
                        int[] route = BruteForceSolveTSP();
                        timerBF.Stop();
                        long elapsedTicksBF = timerBF.ElapsedTicks;
                        Console.WriteLine($"Time (in ticks): {elapsedTicksBF}");
                        PrintSolution(route);
                        break;
                    case 2:
                        bestRoute = new int[numCities];
                        minDistance = int.MaxValue;
                        Stopwatch timerBB = new Stopwatch();
                        timerBB.Start();
                        int[] routeBB = BranchAndBoundSolveTSP();
                        timerBB.Stop();
                        long elapsedTicksBB = timerBB.ElapsedTicks;
                        Console.WriteLine($"Time (in ticks): {elapsedTicksBB}");
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

    static void ParseDataAndCalculateDistances(string data)
    {
        string[] lines = data.Split('\n');
        numCities = lines.Length;
        distances = new int[numCities, numCities];

        for (int i = 0; i < numCities; i++)
        {
            string[] parts = lines[i].Split(' ');
            int x1 = int.Parse(parts[0]);
            int y1 = int.Parse(parts[1]);
            for (int j = 0; j < i; j++)
            {
                string[] parts2 = lines[j].Split(' ');
                int x2 = int.Parse(parts2[0]);
                int y2 = int.Parse(parts2[1]);
                int distance = CalculateDistance(x1, y1, x2, y2);
                distances[i, j] = distance;
                distances[j, i] = distance;
            }
            distances[i, i] = 0; // Distance from a city to itself is 0
        }
    }

    static int CalculateDistance(int x1, int y1, int x2, int y2)
    {
        // Using Euclidean distance formula
        return (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

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

    static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    static int CalculateTotalDistance(int[] route)
    {
        int totalDistance = 0;
        for (int i = 0; i < numCities - 1; i++)
        {
            totalDistance += distances[route[i], route[(i + 1) % numCities]]; // Use modulo to wrap around to the starting city
        }
        totalDistance += distances[route[numCities - 1], route[0]]; // Return to the starting city
        return totalDistance;
    }

    static int CalculateLowerBound(int[] route, int level)
    {
        int lowerBound = 0;
        for (int i = 0; i < level - 1; i++)
        {
            lowerBound += distances[route[i], route[i + 1]];
        }
        return lowerBound;
    }

    static void PrintSolution(int[] route)
    {
        Console.WriteLine("Shortest route: " + string.Join(" -> ", route) + " -> 0");
        Console.WriteLine("Total distance: " + CalculateTotalDistance(route));
    }
}
