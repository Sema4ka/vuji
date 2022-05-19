using System.Collections.Generic;
using GameSettings;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ManagerGame : MonoBehaviourPunCallbacks
{
    private GameObject[] _playerGameObjectsList;
    private bool _allPlayersWasSpawnedInGame;

    #region Unity Methods

    private void Start()
    {
        _allPlayersWasSpawnedInGame = false;
        gameObject.GetComponent<SpawnPlayers>().enabled = true;
        DistributionByTeams();
    }

    private void Update()
    {
        if (!_allPlayersWasSpawnedInGame)
        {
            AllPlayersWasSpawned();
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Проверка на то что все игроки заспавнены
    /// </summary>
    private void AllPlayersWasSpawned()
    {
        _playerGameObjectsList = GameObject.FindGameObjectsWithTag("Player");
        if (_playerGameObjectsList.Length == GameSettingsOriginal.MaxPlayersInGame)
        {
            _allPlayersWasSpawnedInGame = true;
            WhenAllPlayersSpawned();
        }
    }

    /// <summary>
    /// Когда все игроки были заспавнены
    /// </summary>
    private void WhenAllPlayersSpawned()
    {
        gameObject.GetComponent<EndGame>().enabled = true;
    }

    /// <summary>
    /// Распределение игроков по командам 
    /// </summary>
    private void DistributionByTeams()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonTeamsManager teams = gameObject.GetComponent<PhotonTeamsManager>();
            var teamNames = new List<string>();
            foreach (var p in PhotonNetwork.PlayerList)
            {
                var pTeam = p.CustomProperties["team"].ToString();
                if (!teamNames.Contains(pTeam) && pTeam != "None")
                {
                    teamNames.Add(pTeam);
                }
            }

            if (teamNames.Count == 0)
            {
                var count = 0;
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if (count < GameSettingsOriginal.MaxPlayersInGame / 2)
                    {
                        player.JoinTeam("TeamOne");
                        count++;
                    }
                    else
                    {
                        player.JoinTeam("TeamTwo");
                    }
                }
            }
            else if (teamNames.Count == 1)
            {
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    var playerTeam = player.CustomProperties["team"].ToString();
                    if (playerTeam != "None")
                    {
                        player.JoinTeam("TeamOne");
                    }
                    else
                    {
                        player.JoinTeam("TeamTwo");
                    }
                }
            }
            else if (teamNames.Count == 2)
            {
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    var playerTeam = player.CustomProperties["team"].ToString();
                    if (playerTeam == teamNames[0])
                    {
                        player.JoinTeam("TeamOne");
                    }
                    else
                    {
                        player.JoinTeam("TeamTwo");
                    }
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
        gameObject.GetComponent<SpawnEnemy>().enabled = true;
        //gameObject.GetComponent<SpawnPlayers>().enabled = true;
    }

    #endregion
}