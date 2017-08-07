using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public Graph graph;
    public TAListContainer TAListContainer;

    void Awake () {
        Instance = this;
        TAListContainer = FindObjectOfType<TAListContainer>();
        TADeserialize taDeserialize = new TADeserialize();
        taDeserialize.InitializeAsync();

        TAListItem item = TAListContainer.AddItem();
        item.ShowGraphElement(graph.Nodes.First().Value.obj);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
