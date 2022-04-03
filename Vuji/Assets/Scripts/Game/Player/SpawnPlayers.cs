using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerGameObject;
    void Awake()
    {
        Vector2 position = new Vector2(5, 5);
        PhotonNetwork.Instantiate(playerGameObject.name, position, Quaternion.identity);
    }

    
}