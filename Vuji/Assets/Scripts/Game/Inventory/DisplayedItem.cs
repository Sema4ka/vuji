using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Модуль для управления отображаемым предметом инвентаря
/// </summary>
public class DisplayedItem : EventTrigger
{
    public RectTransform inventoryPanel; // Целевая панель инвентаря
    public RectTransform displayedItem; // Целевая панель предмета
    public int itemId; // ID предмета
    public Vector2 itemPosition; // сохраненная позиция предмета на экране

    public static Action<int> onItemDrop; // Событие выбрасывания предмета из инвентаря
    public static Action<DisplayedItem> onItemSwap; // Событие изменения позиции предмета в инвентаре

    private bool isDragging; // Индикатор изменения позиции предмета на экране



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
    /// <summary>
    /// При зажатии курсором панели предмета
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        itemPosition = displayedItem.position;
        isDragging = true;
    }
    /// <summary>
    /// При отжатии курсора
    /// </summary>
    /// <param name="eventData"></param>
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
