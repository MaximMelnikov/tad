using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

public static class ProfileManager
{
    static Profile _currentProfile;
    public static Profile currentProfile
    {
        get
        {
            if (_currentProfile == null)
            {
                _currentProfile = LoadProfile();
            }
            return _currentProfile;
        }
        set { _currentProfile = value; }
    }

    public static void SaveProfile()
    {
        var serializer = new XmlSerializer(typeof(Profile));
        using (var stream = new FileStream(StandardPaths.saveDataDirectory + "/save.xml", FileMode.Create))
        {
            serializer.Serialize(stream, currentProfile);
        }
    }

    public static Profile LoadProfile()
    {
        var serializer = new XmlSerializer(typeof(Profile));
        using (var stream = new FileStream(StandardPaths.saveDataDirectory + "/save.xml", FileMode.Open))
        {
            return serializer.Deserialize(stream) as Profile;
        }
    }
}

[Serializable]
public struct KeyValuePairSerializable<K, V>
{
    public KeyValuePairSerializable(KeyValuePair<K, V> pair)
    {
        Key = pair.Key;
        Value = pair.Value;
    }

    [XmlAttribute]
    public K Key { get; set; }

    [XmlText]
    public V Value { get; set; }
}
[XmlRoot]
public class Profile
{
    public string currentAdventure;
    public string currentNodeId;
    public string lastCheckpoint;
    [XmlIgnore]
    public Dictionary<string, string> values { get; set; }

    [XmlArray("Values")]
    [XmlArrayItem("Pair")]
    public KeyValuePairSerializable<string, string>[] valuesXml
    {
        get
        {
            if (values != null)
            {
                return values.Select(p => new KeyValuePairSerializable<string, string>(p)).ToArray();
            }
            return null;
        }
        set
        {
            values = value.ToDictionary(i => i.Key, i => i.Value);
        }
    }


    public void SaveXml()
    {
        var serializer = new XmlSerializer(typeof(Profile));
        using (var stream = new FileStream(StandardPaths.saveDataDirectory + "/save.xml", FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public void SetValue(string variable, string value)
    {
        if (values.ContainsKey(variable))
        {
            values.Remove(variable);
        }
        values.Add(variable, value);
        SaveXml();
    }

    public void AddCheckpoint(string id)
    {
        lastCheckpoint = id;
    }

    public string GetValue(string variable)
    {
        string s = string.Empty;
        values.TryGetValue(variable, out s);
        return s;
    }
}