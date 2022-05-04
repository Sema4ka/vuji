using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerGameObject;
    [SerializeField] private Transform spawnPointTeamOne;
    [SerializeField] private Transform spawnPointTeamTwo;
    private Vector3 _position;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (playerGameObject != null)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.Equals(PhotonNetwork.LocalPlayer))
                {
                    if (player.GetPhotonTeam().Name == "TeamOne")
                    {
                        _position = spawnPointTeamOne.position;
                    }
                    else if (player.GetPhotonTeam().Name == "TeamTwo")
                    {
                        _position = spawnPointTeamTwo.position;
                    }
                }
            }
            PhotonNetwork.Instantiate(playerGameObject.name, _position, Quaternion.identity);
        }
    }
}