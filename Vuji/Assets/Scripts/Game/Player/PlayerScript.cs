using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public void KillPlayer()
    {
        gameObject.SetActive(false);
        //PhotonNetwork.Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            this.gameObject.GetComponent<Inventory>().AddItem(other.gameObject.GetComponent<DroppedItem>().itemData);
            Destroy(other.gameObject);
        }
    }
}
