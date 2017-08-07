using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TAListContainer : MonoBehaviour {
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
        
        item.SetRow( lastItemNum++ );
        TAScroll.topBound = itemsList.Peek().rectTransform.anchoredPosition.y;
        TAScroll.downBound = item.rectTransform.anchoredPosition.y + canvasRectTransform.rect.height - 20 - item.rectTransform.rect.height;

        cameraTransform.anchoredPosition = new Vector2(0, TAScroll.downBound);        
        return item;
    }
}