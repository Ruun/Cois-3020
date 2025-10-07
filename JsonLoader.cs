

using System.Text.Json;

/// <summary>
/// Handles loading graph data from JSON files
/// Provides functionality to parse JSON data and populate the graph structure
/// </summary>
public class JsonLoader
{
    /// <summary>
    /// Loads graph data from a JSON file and populates the graph structure
    /// Reads node and edge information to build the complete location network
    /// </summary>
    /// <param name="graph">The graph instance to populate with data</param>
    /// <exception cref="FileNotFoundException">Thrown when JSON file is not found</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails</exception>
    public static void LoadGraphData(Graph graph)
    {
        // Read the entire JSON file content into a string
        string json = File.ReadAllText("GraphData.json");
        
        // Deserialize JSON string into JsonData object
        // This converts JSON structure to C# objects for easy processing
        var data = JsonSerializer.Deserialize<JsonData>(json);

        // Add all nodes to the graph
        // Each node represents a location in the network
        foreach (string nodeName in data.nodes)
        {
            graph.AddNode(nodeName);
        }

        // Add all edges to the graph
        // Each edge represents a directed connection between locations with travel time
        foreach (var edge in data.edges)
        {
            graph.AddEdge(edge.source, edge.target, edge.time_min);
        }

        Console.WriteLine("JSON data loaded successfully!");
    }
}

/// <summary>
/// Represents the complete graph data structure from JSON
/// Maps directly to the JSON file structure for deserialization
/// </summary>
public class JsonData
{
    /// <summary>
    /// Array of location names to be added as nodes in the graph
    /// </summary>
    public string[] nodes { get; set; }
    
    /// <summary>
    /// Array of edges representing connections between locations
    /// </summary>
    public EdgeData[] edges { get; set; }
}

/// <summary>
/// Represents a single directed edge between two locations
/// Contains source, target, and travel time information
/// </summary>
public class EdgeData
{
    /// <summary>
    /// The starting location of the edge (source node)
    /// </summary>
    public string source { get; set; }
    
    /// <summary>
    /// The ending location of the edge (destination node)
    /// </summary>
    public string target { get; set; }
    
    /// <summary>
    /// Travel time in minutes between source and target locations
    /// This weight represents the cost of traversing this edge
    /// </summary>
    public int time_min { get; set; }
}
