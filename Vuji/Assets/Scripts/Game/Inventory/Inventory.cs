using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Inventory : MonoBehaviour
{
    public Action<BaseItem> onItemAdded;

    [SerializeField] public List<BaseItem> inventoryItems = new List<BaseItem>();
    [SerializeField] private GameObject _droppedItemPrefab;

    PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

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


    [PunRPC]
    public void SyncronisedDrop(int index)
    {
        var item = inventoryItems[index];
        item.SetAmount(item.GetAmount() - 1);

        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1f);
        GameObject droppedItem = Instantiate(_droppedItemPrefab, position, Quaternion.identity);
        droppedItem.GetComponent<DroppedItem>().SetItem(item);
    }

    public bool DropItem(int itemId)
    {
        _view.RPC("SyncronisedDrop", RpcTarget.All, itemId);

        var item = inventoryItems[itemId];
        if (item.GetAmount() < 1)
        {
            return false;
        }

        return true;
    }
}