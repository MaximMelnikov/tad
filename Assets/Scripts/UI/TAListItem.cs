using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TAListItem : MonoBehaviourEX
{
    [HideInInspector]
    public RectTransform rectTransform;
    [HideInInspector]
    public Text title;
    [HideInInspector]
    public Text text;
    [HideInInspector]
    public List<Button> buttons;
    [HideInInspector]
    public Button buttonPrefab;
    [HideInInspector]
    public LayoutGroup buttonsLayout;
    const float timeToNext = 1f;

    public void Init() {
        rectTransform = GetComponent<RectTransform>();
        Vector2 listSize = transform.parent.GetComponent<RectTransform>().sizeDelta;
        rectTransform.sizeDelta = new Vector2(listSize.x, listSize.y / 6);
        gameObject.SetActive(true);
        buttonPrefab = GetComponentInChildren<Button>();
        buttonsLayout = GetComponentInChildren<LayoutGroup>();
        text = transform.Find("text").GetComponent<Text>();
        title = transform.Find("title").GetComponent<Text>();
    }

    public void SetRow(int row) {
        rectTransform.anchoredPosition = new Vector2(0, 0 - rectTransform.rect.height * row);
    }

    public void ShowGraphElement(BaseNode node)
    {
        if (node.type == "End")
        {
            EndNode endNode = (EndNode)node;
            //the end
        }
        else if (node.type == "Set")
        {
            SetNode setNode = (SetNode)node;
            ProfileManager.currentProfile.SetValue(setNode.variable, setNode.value);
            AddTimer(timeToNext, ()=> { ShowGraphElement(GameManager.Instance.graph.GetNode(setNode).GetConnection()); });
        }
        else if (node.type == "Checkpoint")
        {
            CheckpointNode checkpointNode = (CheckpointNode)node;
            ProfileManager.currentProfile.AddCheckpoint(node.id);
            AddTimer(timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(checkpointNode).GetConnection()); });
        }
        else if (node.type == "Deadend")
        {
            DeadendNode deadendNode = (DeadendNode)node;
            //return to previous checkpoint
        }
        else if (node.type == "Branch")
        {
            BranchNode deadendNode = (BranchNode)node;
            string value = ProfileManager.currentProfile.GetValue(deadendNode.variable);

            /*if (deadendNode.branches == 2)
            {
                Debug.Log("Can't be only one variant in Branch");
            }
            else
            {
                for (int i = 0; i < node.branches.length; i++)
                {
                    if (node.value == node.branches[i])
                    {
                        if (i == node.branches.lenght - 1)
                        {
                            ShowGraphElement(node.branches[node.branches.length]);
                        }
                        else
                        {
                            ShowGraphElement(node.branches[i]);
                        }
                    }
                }
            } */           
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
            AddTimer(timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(startNode).GetConnection()); });
        }
        else if (node.type == "Choice")
        {
            ChoiceNode choiceNode = (ChoiceNode)node;
            AddTimer(timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(choiceNode).GetConnection()); });
        }
    }

    public void SetText(TextNode textNode)
    {
        title.text = textNode.actor;
        text.text = textNode.name;

        if (textNode.choices != null && textNode.choices.Length > 0)
        {
            ShowButtons(textNode.choices);
        }
        else
        {
            AddTimer(timeToNext, () => { ShowGraphElement(GameManager.Instance.graph.GetNode(textNode).GetConnection()); });
        }
    }

    public void ShowButtons(string[] buttonsIds)
    {
        foreach (var i in buttonsIds)
        {
            //AddItemButton(i);
        }
    }

    public void PressBtn(string id)
    {
        //ShowGraphElement(node.branches[i]);
    }
}