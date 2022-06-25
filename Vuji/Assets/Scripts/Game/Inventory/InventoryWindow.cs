using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// Модуль управления отображаемым инвентарем сущности
/// </summary>
public class InventoryWindow : MonoBehaviour
{
    private GameObject player; // Объект целевой сущности
    [SerializeField] Inventory playerInventory; // Модуль инвентаря целевой сущности
    [SerializeField] RectTransform inventoryPanel; // Целевая панель инвентаря



    List<GameObject> displayedIcons = new List<GameObject>(); // Список отображаемых предметов

    public void Start()
    {
        if (playerInventory != null) playerInventory.onItemAdded += OnItemAdded;
        DisplayedItem.onItemDrop += OnItemDropped;
        DisplayedItem.onItemSwap += onItemSwapped;
        KeyHandler.keyPressed += KeyPressed;
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        DisplayedItem.onItemDrop -= OnItemDropped;
        DisplayedItem.onItemSwap -= onItemSwapped;
        KeyHandler.keyPressed -= KeyPressed;
        SpawnPlayers.OnSpawn -= OnSpawn;
        if (playerInventory != null) playerInventory.onItemAdded -= OnItemAdded;
    }

    public void OnSpawn(GameObject playerObject)
    {
        player = playerObject;
        playerInventory = playerObject.GetComponent<Inventory>();
        playerInventory.onItemAdded += OnItemAdded;
        Redraw();
    }

    private void Update()
    {

    }

    void onItemSwapped(DisplayedItem item) => OnItemSwap(item);
    void OnItemDropped(int itemId) => OnItemDrop(itemId);
    void OnItemAdded(BaseItem item) => Redraw();
    /// <summary>
    /// Функция для события нажатия ключа действия
    /// </summary>
    /// <param name="name">Название действия</param>
    /// <param name="key">Ключ действия</param>
    void KeyPressed(string name, KeyCode key)
    {
        string[] words = name.Split(' ');
        if (words[0] != "Slot") return;
        int num = Convert.ToInt32(words[1]) - 1;
        if (displayedIcons.Count() > num)
        {
            bool stillHas = playerInventory.inventoryItems[num].UseItem(player);
            if (!stillHas)
            {
                playerInventory.inventoryItems.RemoveAt(num);
            }
            Redraw();
        }
    }
    /// <summary>
    /// Функция для события изменения позиции предмета в инвентаре
    /// </summary>
    /// <param name="item">Целевой предмет</param>
    void OnItemSwap(DisplayedItem item)
    {
        bool swapped = false;
        foreach (GameObject icon in displayedIcons)
        {
            RectTransform iconTransform = icon.GetComponent<RectTransform>();
            Vector2 mousePos = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(iconTransform, mousePos))
            {
                int newId = icon.GetComponent<DisplayedItem>().itemId;
                int oldId = item.itemId;
                if (newId == oldId) continue;
                BaseItem swapping = playerInventory.inventoryItems[newId];
                playerInventory.inventoryItems[newId] = playerInventory.inventoryItems[oldId];
                playerInventory.inventoryItems[oldId] = swapping;
                swapped = true;
                break;
            }
        }
        if (swapped) Redraw();
        else
        {
            item.transform.position = item.itemPosition;
        }
    }
    /// <summary>
    /// Функция для события выбрасывания премета из инвентаря
    /// </summary>
    /// <param name="itemId">ID выбрасываемого предмета</param>
    void OnItemDrop(int itemId)
    {
        if (displayedIcons.Count > itemId && itemId >= 0)
        {
            if (!playerInventory.DropItem(itemId))
            {
                playerInventory.inventoryItems.RemoveAt(itemId);
            }
            Redraw();
        }
    }
    /// <summary>
    /// Обновить отображаемые предметы
    /// </summary>
    void Redraw()
    {
        ClearDisplayedItems();
        for (var i = 0; i < playerInventory.inventoryItems.Count; i++)
        {
            var item = playerInventory.inventoryItems[i];

            if (item != null)
            {
                var icon = new GameObject(name: "Icon");
                icon.AddComponent<Image>().sprite = item.GetImage();
                icon.AddComponent<DisplayedItem>().displayedItem = icon.GetComponent<RectTransform>();
                icon.GetComponent<DisplayedItem>().inventoryPanel = inventoryPanel;
                icon.GetComponent<DisplayedItem>().itemId = i;
                icon.AddComponent<TooltipTextUI>().tooltipName = item.GetItemName();
                icon.transform.SetParent(inventoryPanel);
                displayedIcons.Add(icon);
            }
        }
    }

    void ClearDisplayedItems()
    {
        for (var i = 0; i < displayedIcons.Count; i++)
        {
            Destroy(displayedIcons[i]);
        }
        displayedIcons.Clear();
    }
}
