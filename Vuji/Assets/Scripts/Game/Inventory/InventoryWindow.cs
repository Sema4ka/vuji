using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] Image highlight;
    [SerializeField] Inventory playerInventory;
    [SerializeField] RectTransform inventoryPanel;
    private int currentItemIndex = 0;

    List<GameObject> displayedIcons = new List<GameObject>();

    public void Start()
    {
        playerInventory.onItemAdded += OnItemAdded;
        Redraw();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BaseItem currentItem = playerInventory.inventoryItems[currentItemIndex];
            bool dropAll = Input.GetKeyDown(KeyCode.LeftControl);
            bool hasItem = currentItem.DropItem(dropAll);
            if (!hasItem)
            {
                playerInventory.inventoryItems.Remove(currentItem);
                currentItemIndex--;
                if (currentItemIndex < 0)
                {
                    currentItemIndex = 0;
                }
            }
            Redraw();
        }


        if (displayedIcons.Count < 1) return;


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentItemIndex = (currentItemIndex + 1);
            if (currentItemIndex > playerInventory.inventoryItems.Count - 1)
            {
                currentItemIndex = 0;
            }
            else if (currentItemIndex < 0)
            {
                currentItemIndex = playerInventory.inventoryItems.Count - 1;
            }
            Redraw();
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentItemIndex = (currentItemIndex - 1);
            if (currentItemIndex > playerInventory.inventoryItems.Count - 1)
            {
                currentItemIndex = 0;
            }
            else if (currentItemIndex < 0)
            {
                currentItemIndex = playerInventory.inventoryItems.Count - 1;
            }
            Redraw();
        }
    }


    void OnItemAdded(BaseItem item) => Redraw();

    void Redraw()
    {
        highlight.transform.SetParent(inventoryPanel);
        highlight.gameObject.SetActive(false);
        ClearDisplayedItems();
        for (var i = 0; i < playerInventory.inventoryItems.Count; i++)
        {
            var item = playerInventory.inventoryItems[i];
            
            if (item != null)
            {
                var icon = new GameObject(name: "Icon");
                icon.AddComponent<Image>().sprite = item.GetImage();
                icon.transform.SetParent(inventoryPanel);
                if (i == currentItemIndex)
                {
                    if (!highlight.gameObject.activeSelf)
                    {
                        highlight.gameObject.SetActive(true);
                    }
                    highlight.transform.SetParent(icon.transform);
                    
                    highlight.transform.position = icon.transform.position;
                }
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
