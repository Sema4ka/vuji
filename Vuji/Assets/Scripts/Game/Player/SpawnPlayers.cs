using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerGameObject;

    void Start()
    {
        Vector2 position = new Vector2(5, 5);
        PhotonNetwork.Instantiate(playerGameObject.name, position, Quaternion.identity);
    }
}