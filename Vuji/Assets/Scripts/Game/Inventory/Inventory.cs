using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _droppedItemPrefab;
    public Action<BaseItem> onItemAdded;

    [SerializeField] public List<BaseItem> inventoryItems = new List<BaseItem>();

    public void AddItem(BaseItem item, GameObject itemGameObject){
        inventoryItems.Add(item);
        onItemAdded?.Invoke(item);
        Debug.Log("Added item: " + item.GetItemName() + item.GetDescription() +  item.GetAmount());
        Destroy(itemGameObject);
    }

    public List<BaseItem> GetAllItems(){
        return inventoryItems;
    }
    public void ClearInventory()
    {
        inventoryItems.Clear();
    }
}