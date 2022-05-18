using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject goblinGameObject2;
    public GameObject goblinSeekerGameObject2;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(goblinGameObject2.name, new Vector3(2, 0, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(2, -7, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(8, -7, 0), Quaternion.identity);
        }
    }
}