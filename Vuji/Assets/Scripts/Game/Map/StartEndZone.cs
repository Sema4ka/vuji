using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndZone : MonoBehaviour
{

    [SerializeField] private BaseItem item;
    private void OnTriggerEnter(Collider player)
    {
        List<BaseItem> inventory = player.GetComponent<Inventory>().GetComponent<List<BaseItem>>();
        if (inventory.Contains(item))
        {
            //�������� �����
        }
    }
}
