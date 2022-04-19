using System;
using System.Collections.Generic;
using GameSettings;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ManagerGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject spawnPlayers;
    private GameObject[] _playerGameObjectsList;
    private bool _allPlayersInGame = false;

    #region Unity Methods

    private void Awake()
    {
        DistributionByTeams();
    }

    private void Update()
    {
        if (!_allPlayersInGame)
        {
            _playerGameObjectsList = GameObject.FindGameObjectsWithTag("Player");
            int livePlayersNow = 0;
            foreach (var p in _playerGameObjectsList)
            {
                if (p.activeSelf) livePlayersNow++;
            }

            if (livePlayersNow == GameSettingsOriginal.MaxPlayersInGame)
            {
                _allPlayersInGame = true;
            }
        }
        else
        {
            gameObject.GetComponent<EndGame>().enabled = true;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Распределение игроков по командам 
    /// </summary>
    private void DistributionByTeams()
    {
        var teamNames = new List<string>();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            string playerTeam = player.CustomProperties["team"].ToString();


            if (!teamNames.Contains(playerTeam) && playerTeam != "None")
            {
                teamNames.Add(playerTeam);
            }
        }

        if (teamNames.Count == 0)
        {
            var count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (count < GameSettingsOriginal.MaxPlayersInGame / 2)
                {
                    player.JoinTeam(1);
                    count++;
                }
                else
                {
                    player.JoinTeam(2);
                }
            }
        }
        else if (teamNames.Count == 1)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                string playerTeam = player.CustomProperties["team"].ToString();
                if (playerTeam != "None")
                {
                    player.JoinTeam(1);
                }
                else
                {
                    player.JoinTeam(2);
                }
            }
        }
        else if (teamNames.Count == 2)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                string playerTeam = player.CustomProperties["team"].ToString();
                if (playerTeam == teamNames[0])
                {
                    player.JoinTeam(1);
                }
                else
                {
                    player.JoinTeam(2);
                }
            }
        }
    }

    /// <summary>
    /// Метод возвращает кол-во игроков в команде
    /// </summary>
    /// <param name="teamName">название команды (TeamOne/TeamTwo)</param>
    /// <returns></returns>
    private int GetTeamMembersNum(string teamName)
    {
        Player[] players;
        if (PhotonTeamsManager.Instance.TryGetTeamMembers(teamName, out players))
        {
            return players.Length;
        }

        return 0;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// После того как игроки будут распределны по командам - заспавнить их
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        spawnPlayers.SetActive(true);
    }

    #endregion
}