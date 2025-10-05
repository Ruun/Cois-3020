// See https://aka.ms/new-console-template for more information


class Program
{
    static void Main()
    {
        Console.WriteLine("🚗 COIS 3020 - Location Route Finder");
        Console.WriteLine("=====================================\n");

        Graph graph = new Graph();
        JsonLoader.LoadGraphData(graph);

        while (true)
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("1. Find shortest route between locations");
            Console.WriteLine("2. Show all locations and connections");
            Console.WriteLine("3. List all locations");
            Console.WriteLine("4. Exit");
            Console.Write("\nChoose option (1-4): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    FindRoute(graph);
                    break;
                case "2":
                    graph.PrintGraph();
                    break;
                case "3":
                    ShowAllLocations(graph);
                    break;
                case "4":
                    Console.WriteLine("Thank you for using Route Finder! Goodbye!");
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please enter 1-4.");
                    break;
            }
        }
    }

    static void FindRoute(Graph graph)
    {
        Console.WriteLine("\n--- FIND SHORTEST ROUTE ---");

        var locations = graph.GetAllNodes().OrderBy(n => n).ToList();
        Console.WriteLine("Available locations:");
        foreach (var location in locations)
        {
            Console.WriteLine($"  {location}");
        }

        Console.Write("\nEnter START location: ");
        string startInput = Console.ReadLine()?.Trim();
        string start = FindMatchingLocation(locations, startInput);

        Console.Write("Enter END location: ");
        string endInput = Console.ReadLine()?.Trim();
        string end = FindMatchingLocation(locations, endInput);

        // Validate input
        if (start == null)
        {
            Console.WriteLine($"❌ Error: '{startInput}' not found. Please use full location name.");
            return;
        }

        if (end == null)
        {
            Console.WriteLine($"❌ Error: '{endInput}' not found. Please use full location name.");
            return;
        }

        if (start == end)
        {
            Console.WriteLine("✅ You are already at the destination!");
            return;
        }

        // Find and display path
        var result = graph.FindShortestPath(start, end);

        if (result.path.Count == 0 || result.totalTime == -1)
        {
            Console.WriteLine($"❌ No route found from {start} to {end}");
        }
        else
        {
            Console.WriteLine($"\n✅ ROUTE FOUND!");
            Console.WriteLine($"🛣️  Path: {string.Join(" → ", result.path)}");
            Console.WriteLine($"⏱️  Total time: {result.totalTime} minutes");
            Console.WriteLine($"🛑 Stops: {result.path.Count - 1}");
        }
    }

    static string FindMatchingLocation(System.Collections.Generic.List<string> locations, string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // Try exact match first
        var exactMatch = locations.FirstOrDefault(l => l.Equals(input, StringComparison.OrdinalIgnoreCase));
        if (exactMatch != null)
            return exactMatch;

        // Try partial match (like "A" for "Location A")
        var partialMatch = locations.FirstOrDefault(l => l.EndsWith(input, StringComparison.OrdinalIgnoreCase));
        if (partialMatch != null)
            return partialMatch;

        return null;
    }

    static void ShowAllLocations(Graph graph)
    {
        var locations = graph.GetAllNodes().OrderBy(n => n).ToList();
        Console.WriteLine("\n--- ALL LOCATIONS ---");
        foreach (var location in locations)
        {
            Console.WriteLine($"📍 {location}");
        }
        Console.WriteLine($"Total: {locations.Count} locations");
    }
}
