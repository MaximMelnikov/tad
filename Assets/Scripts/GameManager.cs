﻿using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public const float timeToNext = 1f;
    public Graph graph;
    public TAListContainer TAListContainer;
    public TADeserialize taDeserialize;

    void Awake () {
        Instance = this;
        taDeserialize = new TADeserialize();
        taDeserialize.InitializeAsync();

        TAListContainer.ShowGraphElement(graph.Nodes.First().Value);
    }
}
