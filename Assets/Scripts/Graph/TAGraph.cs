using System.Collections.Generic;

public class Graph
{
    public IDictionary<string, BaseNode> Nodes { get; private set; }

    public Graph()
    {
        Nodes = new Dictionary<string, BaseNode>();
    }

    public void AddNode(BaseNode obj)
    {
        if (Nodes.ContainsKey(obj.id))
        {
            return;
        }
        Nodes.Add(obj.id, obj);
    }

    public BaseNode GetNode(BaseNode obj)
    {
        return GetNode(obj.id);
    }

    public BaseNode GetNode(string id)
    {
        BaseNode node;
        Nodes.TryGetValue(id, out node);
        return node;
    }
}