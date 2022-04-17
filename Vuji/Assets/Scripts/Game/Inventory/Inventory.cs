using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<BaseItem> onItemAdded;

    [SerializeField] public List<BaseItem> inventoryItems = new List<BaseItem>();

    public void Start()
    {
    }

    public void AddItem(BaseItem item){
        inventoryItems.Add(item);
        onItemAdded?.Invoke(item);
        Debug.Log("Added item: " + item.GetItemName() + item.GetDescription() +  item.GetAmount());
    }

    public List<BaseItem> GetAllItems(){
        return inventoryItems;
    }
}
