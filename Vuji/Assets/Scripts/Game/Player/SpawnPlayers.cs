using System;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerGameObject = null;

    public static Action<GameObject> OnSpawn;

    [SerializeField] private Transform spawnPointTeamOne;
    [SerializeField] private Transform spawnPointTeamTwo;
    private Vector3 _position;
    private bool canSpawn = false;

    private void Start()
    {
        TimerManager.timerEnd += OnTimerEnd;
    }

    void OnTimerEnd(bool ended)
    {
        canSpawn = ended;
    }
    private void Update()
    {
        if (playerGameObject != null && canSpawn)
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
            GameObject playerObject = PhotonNetwork.Instantiate(playerGameObject.name, _position, Quaternion.identity);
            OnSpawn?.Invoke(playerObject);
        }
    }
}