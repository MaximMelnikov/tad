using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TADeserialize
{
    bool isInitialized;
    BaseNode[] nodes;

    public BaseNode GetItemById(string id)
    {
        return nodes.FirstOrDefault(s => s.id == id);
    }

    public void InitializeAsync()
    {
        Debug.Log("InitializeAsync");
        if (isInitialized)
            return;

        string json = String.Empty;
        var sdCardPath = StandardPaths.saveDataDirectory;
        var filePath = Path.Combine(sdCardPath, ProfileManager.currentProfile.currentAdventure);

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not exists");
            return;
        }
        var fileContent = File.ReadAllText(filePath);
        json = fileContent;

        nodes = JsonConvert.DeserializeObject<BaseNode[]>(json, new NodeConverter());
        GameManager.Instance.graph = new Graph();

        foreach (var item in nodes)
        {
            GameManager.Instance.graph.AddNode(item);

            if (item.type == "Branch") {
                BranchNode branchNode = (BranchNode)item;
                branchNode.branchNodeVariables = new List<BranchNodeVariables>();
                string str = branchNode.branches.ToString();
                str = str.Replace("{\r\n", "");
                str = str.Replace("\r\n}", "");
                str = str.Replace(",\r\n  ", "%");
                str = str.Replace("\"", "");
                str = str.Replace("\"", "");
                str = str.Replace(": ", ":");
                string[] split = str.Split('%');
                for (int i = 0; i < split.Length - 1; i++) //exclude last (_default) element
                {
                    string[] bnvsplit;
                    if (i == split.Length - 2)
                    {
                        bnvsplit = split[split.Length - 1].Split(':');
                    }
                    else
                    {
                        bnvsplit = split[i].Split(':');
                    }
                    BranchNodeVariables bnv = new BranchNodeVariables();                    
                    bnv.condition = bnvsplit[0];
                    bnv.next = bnvsplit[1];
                    branchNode.branchNodeVariables.Add(bnv);
                }
            }
        }
        int k = 0;
        foreach (var item in nodes)
        {
            var temp = nodes.FirstOrDefault(s => s.id == item.next);
            if (temp != null)
            {
                item.AddConnection(GameManager.Instance.graph.GetNode(temp));
            }
            else
            {
                k++;
                var node = item as TextNode;
                if (node != null && (item.GetType() == typeof(TextNode) && node.choices != null && node.choices.Length > 0))
                {
                    var textNode = item as TextNode;
                    foreach (var j in textNode.choices)
                    {
                        var _temp = nodes.FirstOrDefault(s => s.id == j);
                        if (_temp != null)
                        {
                            item.AddConnection(GameManager.Instance.graph.GetNode(_temp));
                        }
                    }
                }
            }
        }
        isInitialized = true;
    }
}