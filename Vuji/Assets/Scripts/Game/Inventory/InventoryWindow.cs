using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] Inventory playerInventory;
    [SerializeField] RectTransform inventoryPanel;

    private KeyCode[] numbersKeyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    List<GameObject> displayedIcons = new List<GameObject>();

    public void Start()
    {
        playerInventory.onItemAdded += OnItemAdded;
        DisplayedItem.onItemDrop += OnItemDropped;
        Redraw();
    }

    private void Update()
    {
        for (int i = 0; i < 9; i++) {
            if (Input.GetKeyDown(numbersKeyCodes[i]))
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

    void OnItemDropped(int itemId) => OnItemDrop(itemId);
    void OnItemAdded(BaseItem item) => Redraw();

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
