using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item")]
public class BaseItem : ScriptableObject
{
    [SerializeField] private string itemName = "Item";
    [SerializeField] private string description = "Basic Item";
    [SerializeField] private int amount = 1;
    [SerializeField] private Sprite image;

    public void SetAmount(int newAmount)
    {
        if (newAmount < 0)
        {
            return;
        }

        amount = newAmount;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetAmount()
    {
        return amount;
    }

    public Sprite GetImage()
    {
        return image;
    }
    public virtual bool UseItem(GameObject owner)
    {
        // Do smth
        amount = amount - 1;
        if (amount < 1)
        {
            return false;
        }
        return true;
    }
}