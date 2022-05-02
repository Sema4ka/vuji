using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string upgradeDescription;
    [SerializeField] private int upgradeCost;

    public string GetName()
    {
        return upgradeName;
    }
    public string GetDescription()
    {
        return upgradeDescription;
    }
    public int GetCost()
    {
        return upgradeCost;
    }

    public void ApplyUpgrade(BaseEntity player)
    {
        Debug.Log("Upgrade " + upgradeName + " applied to " + player.GetEntityName());
    }
}
