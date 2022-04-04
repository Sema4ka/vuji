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
}
