﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TAListContainer : MonoBehaviourEX
{
    [SerializeField]
    private TAScroll TAScroll;
    private RectTransform cameraTransform;
    [SerializeField]
    private int rowsCount;
    [SerializeField]
    private TAListItem itemPrefab;
    [SerializeField]
    private Queue<TAListItem> itemsList = new Queue<TAListItem>();
    private int lastItemNum;

    private RectTransform canvasRectTransform;
    private RectTransform rectTransform;
    private float height;    

    void Awake () {
        cameraTransform = TAScroll.GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        height = ( ( canvasRectTransform.sizeDelta.y - 20 ) / rowsCount ) * ( rowsCount + 1 );
        rectTransform.sizeDelta = new Vector2( canvasRectTransform.sizeDelta.x, height );
        itemPrefab.gameObject.SetActive( false );        
    }

    public TAListItem AddItem() {
        TAListItem item = null;
        if ( itemsList.Count < 20 ) {
            item = Instantiate( itemPrefab, transform ) as TAListItem;
            item.Init();            
        }
        else {
            item = itemsList.Dequeue();
        }
        itemsList.Enqueue(item);
        item.Clear();
        item.rectTransform.gameObject.SetActive(true);
        item.SetRow( lastItemNum++ );
        TAScroll.topBound = itemsList.Peek().rectTransform.anchoredPosition.y;
        TAScroll.downBound = item.rectTransform.anchoredPosition.y + canvasRectTransform.rect.height - 20 - item.rectTransform.rect.height;

        cameraTransform.anchoredPosition = new Vector2(0, TAScroll.downBound);        
        return item;
    }

    public void RemoveItem()
    {
        TAListItem item = null;
        item = itemsList.Peek();
        itemsList.Dequeue();
        Destroy(item.rectTransform.gameObject);
        Destroy(item);
    }

    public void ShowGraphElement(BaseNode node)
    {
        if (node.type == "End")
        {
            EndNode endNode = (EndNode)node;
        }
        else if (node.type == "Set")
        {
            SetNode setNode = (SetNode)node;
            ProfileManager.currentProfile.SetValue(setNode.variable, setNode.value);
            AddTimer(GameManager.timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(setNode).GetFirstConnection()); });
        }
        else if (node.type == "Checkpoint")
        {
            CheckpointNode checkpointNode = (CheckpointNode)node;
            ProfileManager.currentProfile.AddCheckpoint(node.id);
            AddTimer(GameManager.timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(checkpointNode).GetFirstConnection()); });
        }
        else if (node.type == "Deadend")
        {
            DeadendNode deadendNode = (DeadendNode)node;
            //return to previous checkpoint
        }
        else if (node.type == "Branch")
        {
            BranchNode branchNode = (BranchNode)node;
            string value = ProfileManager.currentProfile.GetValue(branchNode.variable);
            AddTimer(GameManager.timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(branchNode.GetNextStep(value))); });
        }
        else if (node.type == "Text")
        {
            TextNode textNode = (TextNode)node;
            TAListItem item = GameManager.Instance.TAListContainer.AddItem();
            item.SetText(textNode);
        }
        else if (node.type == "Start")
        {
            StartNode startNode = (StartNode)node;
            ShowGraphElement(GameManager.Instance.graph.GetNode(startNode).GetFirstConnection());            
        }
        else if (node.type == "Choice")
        {
            ChoiceNode choiceNode = (ChoiceNode)node;
            AddTimer(GameManager.timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(choiceNode).GetFirstConnection()); });
        }
    }
}