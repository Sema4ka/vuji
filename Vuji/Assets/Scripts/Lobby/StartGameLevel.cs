using System;
using System.Collections;
using GameSettings;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StartGameLevel : MonoBehaviourPunCallbacks
{
    private PlayersFounded _playersFounded;


    #region Unity Methods

    private void Start()
    {
        _playersFounded = gameObject.GetComponent<PlayersFounded>();
    }

    #endregion

    #region Photon Methods

    public override void OnJoinedRoom()
    {
        if (gameObject.GetComponent<LobbyManager>().playerStatus == "SEARCHGAME")
        {
            _playersFounded.ShowPlayersFounded();
            if (CheckPreGameRoom())
            {
                LoadGameLevel();
            }

            _playersFounded.UpdatePlayersFounded();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (gameObject.GetComponent<LobbyManager>().playerStatus == "SEARCHGAME")
        {
            if (CheckPreGameRoom())
            {
                LoadGameLevel();
            }

            _playersFounded.UpdatePlayersFounded();
        }
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        _playersFounded.UpdatePlayersFounded();
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Проверка на нужно кол-во игроков. Если игроков в режиме нужно кол-во возвращает True
    /// </summary>
    /// <returns>True / False</returns>
    private bool CheckPreGameRoom()
    {
        if (PhotonNetwork.PlayerList.Length == GameSettingsOriginal.MaxPlayersInGame)
        {
            return true;
        }
        Debug.Log("Not enough players");
        return false;
    }

    /// <summary>
    /// Загружает сцену Game
    /// </summary>
    private void LoadGameLevel()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    
    #endregion
}