using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _droppedItemPrefab;
    public Action<BaseItem> onItemAdded;

    [SerializeField] public List<BaseItem> inventoryItems = new List<BaseItem>();
    [SerializeField] private GameObject _droppedItemPrefab;
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
}