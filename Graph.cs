

public class Node
{
    public string Name { get; set; }
    public List<Edge> Edges { get; set; }

    public Node(string name)
    {
        Name = name;
        Edges = new List<Edge>();
    }
}

public class Edge
{
    public Node Target { get; set; }
    public int Weight { get; set; }

    public Edge(Node target, int weight)
    {
        Target = target;
        Weight = weight;
    }
}

public class Graph
{
    private Dictionary<string, Node> nodes = new Dictionary<string, Node>();

    public void AddNode(string name)
    {
        if (!nodes.ContainsKey(name))
        {
            nodes[name] = new Node(name);
        }
    }

    public void AddEdge(string from, string to, int weight)
    {
        if (nodes.ContainsKey(from) && nodes.ContainsKey(to))
        {
            nodes[from].Edges.Add(new Edge(nodes[to], weight));
        }
    }

    public Node GetNode(string name)
    {
        return nodes.ContainsKey(name) ? nodes[name] : null;
    }

    public List<string> GetAllNodes()
    {
        return nodes.Keys.ToList();
    }

    public void PrintGraph()
    {
        Console.WriteLine("\n=== GRAPH STRUCTURE ===");
        foreach (var node in nodes.Values.OrderBy(n => n.Name))
        {
            Console.Write($"{node.Name} -> ");
            if (node.Edges.Count == 0)
            {
                Console.Write("No connections");
            }
            else
            {
                foreach (var edge in node.Edges)
                {
                    Console.Write($"{edge.Target.Name}({edge.Weight}min) ");
                }
            }
            Console.WriteLine();
        }
    }

    public (List<string> path, int totalTime) FindShortestPath(string start, string end)
    {
        if (!nodes.ContainsKey(start) || !nodes.ContainsKey(end))
            return (new List<string>(), -1);

        var distances = new Dictionary<string, int>();
        var previous = new Dictionary<string, string>();
        var unvisited = new HashSet<string>();

        foreach (var nodeName in nodes.Keys)
        {
            distances[nodeName] = int.MaxValue;
            previous[nodeName] = null;
            unvisited.Add(nodeName);
        }
        distances[start] = 0;

        while (unvisited.Count > 0)
        {
            string current = null;
            foreach (var node in unvisited)
            {
                if (current == null || distances[node] < distances[current])
                {
                    current = node;
                }
            }

            if (current == end) break;
            if (distances[current] == int.MaxValue) break;

            unvisited.Remove(current);

            foreach (var edge in nodes[current].Edges)
            {
                if (unvisited.Contains(edge.Target.Name))
                {
                    int newDist = distances[current] + edge.Weight;
                    if (newDist < distances[edge.Target.Name])
                    {
                        distances[edge.Target.Name] = newDist;
                        previous[edge.Target.Name] = current;
                    }
                }
            }
        }

        List<string> path = new List<string>();
        string temp = end;
        
        // Check if path exists
        if (previous.ContainsKey(end) && previous[end] == null && start != end)
            return (new List<string>(), -1);

        while (temp != null)
        {
            path.Insert(0, temp);
            temp = previous.ContainsKey(temp) ? previous[temp] : null;
        }

        return (path, distances[end]);
    }
}
