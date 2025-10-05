// See https://aka.ms/new-console-template for more information



/// <summary>
/// Main program class for the COIS 3020 Assignment 1 - Location Route Finder
/// Provides a user interface for interacting with the graph data structure
/// and finding shortest paths between locations
/// </summary>
class Program
{
    /// <summary>
    /// Main entry point of the application
    /// Initializes the graph and runs the main menu loop
    /// </summary>
    static void Main()
    {
        // Display application header
        Console.WriteLine("🚗 COIS 3020 - Location Route Finder");
        Console.WriteLine("=====================================\n");

        // Initialize graph data structure
        Graph graph = new Graph();

        // Load graph data from JSON file
        JsonLoader.LoadGraphData(graph);

        // Main application loop - continues until user chooses to exit
        while (true)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine();

            // Process user menu selection
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

    /// <summary>
    /// Displays the main menu options to the user
    /// </summary>
    static void DisplayMainMenu()
    {
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("1. Find shortest route between locations");
        Console.WriteLine("2. Show all locations and connections");
        Console.WriteLine("3. List all locations");
        Console.WriteLine("4. Exit");
        Console.Write("\nChoose option (1-4): ");
    }

    /// <summary>
    /// Handles the shortest path finding functionality
    /// Guides user through input process and displays results
    /// </summary>
    /// <param name="graph">The graph containing location data</param>
    static void FindRoute(Graph graph)
    {
        Console.WriteLine("\n--- FIND SHORTEST ROUTE ---");

        // Get and display all available locations in alphabetical order
        var locations = graph.GetAllNodes().OrderBy(n => n).ToList();
        Console.WriteLine("Available locations:");
        foreach (var location in locations)
        {
            Console.WriteLine($"  {location}");
        }

        // Get start location from user with flexible input matching
        Console.Write("\nEnter START location: ");
        string startInput = Console.ReadLine()?.Trim();
        string start = FindMatchingLocation(locations, startInput);

        // Get destination location from user
        Console.Write("Enter END location: ");
        string endInput = Console.ReadLine()?.Trim();
        string end = FindMatchingLocation(locations, endInput);

        // Validate user input before proceeding with path finding
        if (!ValidateUserInput(startInput, start, endInput, end))
        {
            return;
        }

        // Calculate and display the shortest path
        DisplayShortestPath(graph, start, end);
    }

    /// <summary>
    /// Validates user input for location names
    /// Checks for existence of locations and prevents same start/end
    /// </summary>
    /// <param name="startInput">Original start location input</param>
    /// <param name="start">Matched start location name</param>
    /// <param name="endInput">Original end location input</param>
    /// <param name="end">Matched end location name</param>
    /// <returns>True if input is valid, False otherwise</returns>
    static bool ValidateUserInput(string startInput, string start, string endInput, string end)
    {
        // Check if start location exists
        if (start == null)
        {
            Console.WriteLine($"❌ Error: '{startInput}' not found. Please use full location name.");
            return false;
        }

        // Check if end location exists
        if (end == null)
        {
            Console.WriteLine($"❌ Error: '{endInput}' not found. Please use full location name.");
            return false;
        }

        // Prevent unnecessary path calculation for same location
        if (start == end)
        {
            Console.WriteLine("✅ You are already at the destination!");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Calculates and displays the shortest path between two locations
    /// </summary>
    /// <param name="graph">Graph containing location data</param>
    /// <param name="start">Starting location name</param>
    /// <param name="end">Destination location name</param>
    static void DisplayShortestPath(Graph graph, string start, string end)
    {
        // Use Dijkstra's algorithm to find shortest path
        var result = graph.FindShortestPath(start, end);

        // Check if a valid path was found
        if (result.path.Count == 0 || result.totalTime == -1)
        {
            Console.WriteLine($"❌ No route found from {start} to {end}");
        }
        else
        {
            // Display successful path finding results
            Console.WriteLine($"\n✅ ROUTE FOUND!");
            Console.WriteLine($"🛣️  Path: {string.Join(" → ", result.path)}");
            Console.WriteLine($"⏱️  Total time: {result.totalTime} minutes");
            Console.WriteLine($"🛑 Stops: {result.path.Count - 1}");
        }
    }

    /// <summary>
    /// Finds a matching location name from user input
    /// Supports both exact matches and partial matches (e.g., "A" for "Location A")
    /// </summary>
    /// <param name="locations">List of all available location names</param>
    /// <param name="input">User input to match against location names</param>
    /// <returns>Matched location name or null if no match found</returns>
    static string FindMatchingLocation(List<string> locations, string input)
    {
        // Return null for empty or whitespace input
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // First attempt: case-insensitive exact match
        var exactMatch = locations.FirstOrDefault(l =>
            l.Equals(input, StringComparison.OrdinalIgnoreCase));
        if (exactMatch != null)
            return exactMatch;

        // Second attempt: case-insensitive partial match
        // Allows inputs like "A" to match "Location A"
        var partialMatch = locations.FirstOrDefault(l =>
            l.EndsWith(input, StringComparison.OrdinalIgnoreCase));
        if (partialMatch != null)
            return partialMatch;

        // No match found
        return null;
    }

    /// <summary>
    /// Displays all available locations in a formatted list
    /// </summary>
    /// <param name="graph">Graph containing location data</param>
    static void ShowAllLocations(Graph graph)
    {
        var locations = graph.GetAllNodes().OrderBy(n => n).ToList();
        Console.WriteLine("\n--- ALL LOCATIONS ---");

        // Display each location with a bullet point
        foreach (var location in locations)
        {
            Console.WriteLine($"📍 {location}");
        }

        Console.WriteLine($"Total: {locations.Count} locations");
    }
}
