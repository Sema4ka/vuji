using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] Inventory playerInventory;
    [SerializeField] RectTransform inventoryPanel;

    

    List<GameObject> displayedIcons = new List<GameObject>();

    public void Start()
    {
        playerInventory.onItemAdded += OnItemAdded;
        DisplayedItem.onItemDrop += OnItemDropped;
        DisplayedItem.onItemSwap += onItemSwapped;
        KeyHandler.keyPressed += KeyPressed;
        Redraw();
    }
   
    private void Update()
    {
        
    }

    void onItemSwapped(DisplayedItem item) => OnItemSwap(item);
    void OnItemDropped(int itemId) => OnItemDrop(itemId);
    void OnItemAdded(BaseItem item) => Redraw();
    void KeyPressed(KeyHandler keyHandler, string name, KeyCode[] keys)
    {
        for (int i = 0; i < 9; i++)
        {
            if (keys.Contains(KeyHandler.numbersKeyCodes[i]))
            {
                if (displayedIcons.Count > i)
                {
                    bool stillHas = playerInventory.inventoryItems[i].UseItem();
                    if (!stillHas)
                    {
                        playerInventory.inventoryItems.RemoveAt(i);
                    }
                    Redraw();
                }
            }
        }
    }

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
    void OnItemDrop(int itemId)
    {
        if (displayedIcons.Count > itemId && itemId >= 0)
        {
            if (!playerInventory.inventoryItems[itemId].DropItem()){
                playerInventory.inventoryItems.RemoveAt(itemId);
            }
            Redraw();
        }
    }

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
                icon.transform.SetParent(inventoryPanel);
                displayedIcons.Add(icon);
            }
        }
    }

    void ClearDisplayedItems()
    {
        for (var i = 0;i < displayedIcons.Count; i++)
        {
            Destroy(displayedIcons[i]);
        }
        displayedIcons.Clear();
    }
}
