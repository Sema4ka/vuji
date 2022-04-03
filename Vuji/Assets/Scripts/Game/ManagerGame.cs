using System;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class ManagerGame : MonoBehaviourPunCallbacks
{
    readonly Action<Player, PhotonTeam> _onSwitchTeam = delegate { };
    private GameObject[] _playerGameObjectsList;
    
    #region Unity Methods

    private void Start()
    {
        SetPlayersInTeams();
    }

    private void Update()
    {
        _playerGameObjectsList = GameObject.FindGameObjectsWithTag("Player");
        if (_playerGameObjectsList.Length < PhotonNetwork.PlayerList.Length)
        {
            Debug.Log("DIED PLAYER");
        }
    }

    #endregion

    #region Private Methods

    private void SetPlayersInTeams()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                player.JoinTeam(1);
            }
            else
            {
                player.JoinTeam(2);
            }
        }
    }

    #endregion

    #region Public Methods

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        object teamCodeObject;
        if (changedProps.TryGetValue(PhotonTeamsManager.TeamPlayerProp, out teamCodeObject))
        {
            if (teamCodeObject == null) return;

            PhotonTeam newTeam;
            if (PhotonTeamsManager.Instance.TryGetTeamByCode((byte) teamCodeObject, out newTeam))
            {
                _onSwitchTeam?.Invoke(targetPlayer, newTeam);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetPlayersInTeams();
    }

    public void KillPlayer(GameObject playerGameObject)
    {
        
    }
    #endregion
}