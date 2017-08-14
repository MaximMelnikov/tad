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

    public void Init() {
        rectTransform = GetComponent<RectTransform>();
        Vector2 listSize = transform.parent.GetComponent<RectTransform>().sizeDelta;
        rectTransform.sizeDelta = new Vector2(listSize.x, listSize.y / 6);
        gameObject.SetActive(true);
        buttonPrefab = transform.Find("Button").GetComponent<Button>();
        buttonsLayout = GetComponentInChildren<LayoutGroup>();
        text = transform.Find("text").GetComponent<Text>();
        title = transform.Find("title").GetComponent<Text>();
    }

    public void SetRow(int row) {
        rectTransform.anchoredPosition = new Vector2(0, 0 - rectTransform.rect.height * row);
    }

    public void SetText(TextNode textNode)
    {
        title.text = textNode.actor;
        text.text = textNode.name;

        if (textNode.choices != null && textNode.choices.Length > 0)
        {
            TAListItem item = GameManager.Instance.TAListContainer.AddItem();
            item.ShowButtons(textNode.choices);
        }
        else
        {
            AddTimer(GameManager.timeToNext, () => { GameManager.Instance.TAListContainer.ShowGraphElement(GameManager.Instance.graph.GetNode(textNode).GetConnection()); });
        }
    }

    public void ShowButtons(string[] buttonsIds)
    {
        foreach (var i in buttonsIds)
        {
            AddItemButton(i);
        }
    }

    public void AddItemButton(string id)
    {
        Button btn = Instantiate(buttonPrefab, buttonsLayout.transform);
        buttons.Add(btn);
        ChoiceNode cn = (ChoiceNode)(GameManager.Instance.taDeserialize.GetItemById(id));
        Text txt = btn.GetComponentInChildren<Text>();
        btn.onClick.AddListener(() => PressBtn(cn.next));
        txt.text = cn.name;
    }

    public void PressBtn(string id)
    {
        GameManager.Instance.TAListContainer.ShowGraphElement(GameManager.Instance.graph.GetNodeById(id));
    }
}