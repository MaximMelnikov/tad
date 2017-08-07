using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class TADeserialize
{
    bool isInitialized;
    BaseNode[] nodes;
        /*public async Task<BaseNode> GetItemAsync(Type nodeType)
        {
            await InitializeAsync();

            return await Task.FromResult(nodes.FirstOrDefault(s => s.GetType() == nodeType));
        }

        public async Task<BaseNode> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(nodes.FirstOrDefault(s => s.id == id));
        }

        public async Task<IEnumerable<BaseNode>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(nodes);
        }*/

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

            if (item.type == "Branch")
            {
                Debug.Log("");
            }
        }
        int k = 0;
        foreach (var item in nodes)
        {
            var temp = nodes.FirstOrDefault(s => s.id == item.next);
            if (temp != null)
            {
                GameManager.Instance.graph.AddConnection(item.id, temp.id, 1);
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
                            GameManager.Instance.graph.AddConnection(item.id, _temp.id, 1);
                        }
                    }
                }
            }
        }
        Debug.Log(nodes[0].id);
        isInitialized = true;
    }
}