


/// <summary>
/// Represents a location node in the graph
/// Each node contains a name and a list of outgoing edges to other nodes
/// </summary>
public class Node
{
    /// <summary>
    /// Name of the location (e.g., "Location A")
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// List of outgoing edges from this node to other locations
    /// </summary>
    public List<Edge> Edges { get; set; }

    /// <summary>
    /// Constructor to initialize a new node with given name
    /// </summary>
    /// <param name="name">The name of the location</param>
    public Node(string name)
    {
        Name = name;
        Edges = new List<Edge>();
    }
}

/// <summary>
/// Represents a directed connection between two locations with travel time
/// </summary>
public class Edge
{
    /// <summary>
    /// The destination node this edge connects to
    /// </summary>
    public Node Target { get; set; }
    
    /// <summary>
    /// Travel time in minutes between the locations
    /// </summary>
    public int Weight { get; set; }

    /// <summary>
    /// Constructor to create a new edge
    /// </summary>
    /// <param name="target">Destination node</param>
    /// <param name="weight">Travel time in minutes</param>
    public Edge(Node target, int weight)
    {
        Target = target;
        Weight = weight;
    }
}

/// <summary>
/// Graph data structure representing a network of locations using adjacency list
/// Implements Dijkstra's algorithm for shortest path finding
/// </summary>
public class Graph
{
    // Dictionary to store all nodes in the graph for quick lookup by name
    private Dictionary<string, Node> nodes = new Dictionary<string, Node>();

    /// <summary>
    /// Adds a new node to the graph if it doesn't already exist
    /// </summary>
    /// <param name="name">Name of the location to add</param>
    public void AddNode(string name)
    {
        // Only add the node if it doesn't already exist to avoid duplicates
        if (!nodes.ContainsKey(name))
        {
            nodes[name] = new Node(name);
        }
    }

    /// <summary>
    /// Creates a directed edge from one location to another with specified travel time
    /// </summary>
    /// <param name="from">Starting location name</param>
    /// <param name="to">Destination location name</param>
    /// <param name="weight">Travel time in minutes</param>
    public void AddEdge(string from, string to, int weight)
    {
        // Only add edge if both source and destination nodes exist
        if (nodes.ContainsKey(from) && nodes.ContainsKey(to))
        {
            nodes[from].Edges.Add(new Edge(nodes[to], weight));
        }
    }

    /// <summary>
    /// Retrieves a node by its name
    /// </summary>
    /// <param name="name">Name of the location to find</param>
    /// <returns>The node if found, null otherwise</returns>
    public Node GetNode(string name)
    {
        return nodes.ContainsKey(name) ? nodes[name] : null;
    }

    /// <summary>
    /// Returns a list of all location names in the graph
    /// </summary>
    /// <returns>List of all node names</returns>
    public List<string> GetAllNodes()
    {
        return nodes.Keys.ToList();
    }

    /// <summary>
    /// Displays the complete graph structure showing all locations and their connections
    /// Useful for debugging and verification
    /// </summary>
    public void PrintGraph()
    {
        Console.WriteLine("\n=== GRAPH STRUCTURE ===");

        // Display nodes in alphabetical order for consistency
        foreach (var node in nodes.Values.OrderBy(n => n.Name))
        {
            Console.Write($"{node.Name} -> ");

            if (node.Edges.Count == 0)
            {
                Console.Write("No connections");
            }
            else
            {
                // Display all outgoing edges with their weights
                foreach (var edge in node.Edges)
                {
                    Console.Write($"{edge.Target.Name}({edge.Weight}min) ");
                }
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Finds the shortest path between two locations using Dijkstra's algorithm
    /// Dijkstra's algorithm is used because it efficiently finds shortest paths 
    /// in weighted graphs with non-negative edge weights
    /// </summary>
    /// <param name="start">Starting location name</param>
    /// <param name="end">Destination location name</param>
    /// <returns>
    /// Tuple containing:
    /// - path: List of location names representing the shortest path
    /// - totalTime: Total travel time in minutes for the shortest path
    /// Returns empty list and -1 if no path exists
    /// </returns>
    public (List<string> path, int totalTime) FindShortestPath(string start, string end)
    {
        // Validate that both start and end locations exist in the graph
        if (!nodes.ContainsKey(start) || !nodes.ContainsKey(end))
            return (new List<string>(), -1);

        // Dijkstra's algorithm data structures
        var distances = new Dictionary<string, int>();    // Shortest known distance to each node
        var previous = new Dictionary<string, string>();  // Previous node in optimal path
        var unvisited = new HashSet<string>();           // Nodes not yet processed

        // Initialize all nodes with infinite distance and no previous node
        foreach (var nodeName in nodes.Keys)
        {
            distances[nodeName] = int.MaxValue;  // Represents infinity
            previous[nodeName] = null;           // No previous node initially
            unvisited.Add(nodeName);             // All nodes start unvisited
        }

        // Distance to start node is 0 (we're already there)
        distances[start] = 0;

        // Process nodes until all are visited or destination is reached
        while (unvisited.Count > 0)
        {
            // Find the unvisited node with the smallest known distance
            string current = null;
            foreach (var node in unvisited)
            {
                if (current == null || distances[node] < distances[current])
                {
                    current = node;
                }
            }

            // If we reached the destination or smallest distance is infinity, we're done
            if (current == end) break;
            if (distances[current] == int.MaxValue) break;

            // Mark current node as visited
            unvisited.Remove(current);

            // Update distances to all neighbors of the current node
            foreach (var edge in nodes[current].Edges)
            {
                // Only consider unvisited neighbors
                if (unvisited.Contains(edge.Target.Name))
                {
                    // Calculate distance to neighbor through current node
                    int newDist = distances[current] + edge.Weight;

                    // If this path is shorter, update the distance and previous node
                    if (newDist < distances[edge.Target.Name])
                    {
                        distances[edge.Target.Name] = newDist;
                        previous[edge.Target.Name] = current;
                    }
                }
            }
        }

        // Reconstruct the shortest path by backtracking from end to start
        List<string> path = new List<string>();
        string temp = end;

        // Check if a valid path exists (end node has a previous node or it's the start node)
        if (previous.ContainsKey(end) && previous[end] == null && start != end)
            return (new List<string>(), -1);

        // Backtrack from end to start using previous pointers
        while (temp != null)
        {
            path.Insert(0, temp);  // Add to front of list to maintain correct order
            temp = previous.ContainsKey(temp) ? previous[temp] : null;
        }

        return (path, distances[end]);
    }
}
