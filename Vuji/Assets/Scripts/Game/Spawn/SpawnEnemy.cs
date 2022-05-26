using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject goblinSeekerGameObject2;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(0, 40, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(12, 38, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(16, 24, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(-5, 24, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(-12, 13, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(-9, -3, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(-5, -12, 0), Quaternion.identity);
            PhotonNetwork.Instantiate(goblinSeekerGameObject2.name, new Vector3(-6, -12, 0), Quaternion.identity);
        }
    }
}