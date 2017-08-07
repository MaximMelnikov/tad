using System;
using System.Collections.Generic;
using System.Linq;

public class Graph
{
    public IDictionary<string, Node> Nodes { get; private set; }

    public Graph()
    {
        Nodes = new Dictionary<string, Node>();
    }

    public void AddNode(BaseNode obj)
    {
        var node = new Node(obj);
        if (!Nodes.ContainsKey(obj.id))
            Nodes.Add(obj.id, node);
    }

    public void AddConnection(string fromNode, string toNode, int distance)
    {
        Nodes[fromNode].AddConnection(Nodes[toNode], distance);
    }

    public Node GetNode(BaseNode obj)
    {
        Node node;
        Nodes.TryGetValue(obj.id, out node);
        return node;
    }
}

public class NodeConnection
{
    internal Node Target { get; private set; }
    internal double Distance { get; private set; }

    internal NodeConnection(Node target, double distance)
    {
        Target = target;
        Distance = distance;
    }
}

public class Node
{
    IList<NodeConnection> _connections;
    internal BaseNode obj;

    internal IEnumerable<NodeConnection> Connections
    {
        get { return _connections; }
    }

    internal Node(BaseNode obj)
    {
        this.obj = obj;
        _connections = new List<NodeConnection>();
    }

    internal void AddConnection(Node targetNode, double distance)
    {
        if (targetNode == null) throw new ArgumentNullException("targetNode");
        if (targetNode == this) throw new ArgumentException("Node may not connect to itself.");
        if (distance <= 0) throw new ArgumentException("Distance must be positive.");

        _connections.Add(new NodeConnection(targetNode, distance));
    }

    internal BaseNode GetConnection()
    {
        return _connections.First().Target.obj;
    }
}
