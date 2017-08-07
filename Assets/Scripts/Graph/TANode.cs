using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
[Serializable]
public abstract class BaseNode
{
    public string type { get; set; }
    public string id { get; set; }
    public string next { get; set; }
}
[Serializable]
public class TextNode : BaseNode
{
    public string actor { get; set; }
    public string name { get; set; }
    public string[] choices { get; set; }
}
[Serializable]
public class ChoiceNode : BaseNode
{
    public string title { get; set; }
    public string name { get; set; }
}
[Serializable]
public class BranchNode : BaseNode
{
    public string variable { get; set; }
    public Object branches { get; set; }
}
[Serializable]
public class BranchNodeVariables
{
    public string num { get; set; }
    public string next { get; set; }
}
[Serializable]
public class SetNode : BaseNode
{
    public string variable { get; set; }
    public string value { get; set; }
}
[Serializable]
public class StartNode : BaseNode
{
    public string name { get; set; }
}
[Serializable]
public class CheckpointNode : BaseNode
{
    public string name { get; set; }
}
[Serializable]
public class DeadendNode : BaseNode
{
    public string name { get; set; }
}
[Serializable]
public class EndNode : BaseNode
{
    public string name { get; set; }
}
public class NodeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(BaseNode).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader,
        Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject item = JObject.Load(reader);

        switch (item["type"].Value<string>())
        {
            case "Text":
                return item.ToObject<TextNode>();
            case "Choice":
                return item.ToObject<ChoiceNode>();
            case "Branch":
                return item.ToObject<BranchNode>();
            case "Set":
                return item.ToObject<SetNode>();
            case "Start":
                return item.ToObject<StartNode>();
            case "Checkpoint":
                return item.ToObject<CheckpointNode>();
            case "Deadend":
                return item.ToObject<DeadendNode>();
            case "End":
                return item.ToObject<EndNode>();
            default:
                return null;
        }
    }

    public override void WriteJson(JsonWriter writer,
        object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}