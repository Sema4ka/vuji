using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<BaseItem> onItemAdded;

    [SerializeField] public List<BaseItem> inventoryItems = new List<BaseItem>();
    [SerializeField] GameObject _droppedItemPrefab;

    public void Start()
    {
    }

    public void AddItem(BaseItem item){
        inventoryItems.Add(item);
        onItemAdded?.Invoke(item);
        Debug.Log("Added item: " + item.GetItemName() + item.GetDescription() +  item.GetAmount());
    }
    public bool DropItem(BaseItem item)
    {
        item.SetAmount(item.GetAmount() - 1);

        Vector2 position = new Vector2(UnityEngine.Random.Range(-3.0f, 3.0f) + gameObject.transform.position.x, UnityEngine.Random.Range(-3.0f, 3.0f) + gameObject.transform.position.y);

        GameObject droppedItem = Instantiate(_droppedItemPrefab, position, Quaternion.identity);
        droppedItem.GetComponent<DroppedItem>().SetItem(item);

        if (item.GetAmount() < 1)
        {
            return false;
        }

        return true;
    }
    public void ClearInventory()
    {
        inventoryItems.Clear();
    }

    public void RemoveItem(BaseItem item)
    {
        inventoryItems.Remove(item);
    }

    public List<BaseItem> GetAllItems(){
        return inventoryItems;
    }
}
