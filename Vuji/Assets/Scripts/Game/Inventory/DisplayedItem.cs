using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayedItem : EventTrigger
{
    public RectTransform inventoryPanel;
    public RectTransform displayedItem;
    public int itemId;
    public Vector2 itemPosition;

    public static Action<int> onItemDrop;
    public static Action<DisplayedItem> onItemSwap;

    private bool isDragging;
    


    // Start is called before the first frame update
    void Start()
    {
        itemPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        itemPosition = displayedItem.position;
        isDragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (rectOverlaps(inventoryPanel, displayedItem))
        {
            onItemDrop?.Invoke(itemId);
        }
        else
        {
            onItemSwap?.Invoke(this);
        }
    }
    public static bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

}
