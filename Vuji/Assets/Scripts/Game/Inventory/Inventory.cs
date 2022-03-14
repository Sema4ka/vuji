using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<BaseItem> inventoryItems = new List<BaseItem>();

    public void AddItem(BaseItem item){
        inventoryItems.Add(item);
        Debug.Log("Added item: " + item.GetItemName() + item.GetDescription() +  item.GetAmount());
    }

    public List<BaseItem> GetAllItems(){
        return inventoryItems;
    }
}