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
        if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, Input.mousePosition))
        {
            onItemDrop?.Invoke(itemId);
        }
        else
        {
            onItemSwap?.Invoke(this);
        }
    }

}
