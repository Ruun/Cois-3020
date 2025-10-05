
using System.Text.Json;

public class JsonLoader
{
    public static void LoadGraphData(Graph graph)
    {
        string json = File.ReadAllText("GraphData.json");
        var data = JsonSerializer.Deserialize<JsonData>(json);

        // Add all nodes
        foreach (string nodeName in data.nodes)
        {
            graph.AddNode(nodeName);
        }

        // Add all edges
        foreach (var edge in data.edges)
        {
            graph.AddEdge(edge.source, edge.target, edge.time_min);
        }

        Console.WriteLine("✅ JSON data loaded successfully!");
    }
}

public class JsonData
{
    public string[] nodes { get; set; }
    public EdgeData[] edges { get; set; }
}

public class EdgeData
{
    public string source { get; set; }
    public string target { get; set; }
    public int time_min { get; set; }
}
