using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class Play : MonoBehaviourPunCallbacks
{
    private string _masterClientIDGame;
    // 0 - do not start search game room
    // 1 - [SOLO]
    // 2 - [TEAM] you master client
    // 3 - [TEAM] you find master step 1/2
    // 4 - [TEAM] you find master step 2/2
    private int _startMode;
    private readonly RoomOptions _roomOptions = new RoomOptions {MaxPlayers = 4, IsOpen = true, IsVisible = true};
    private readonly List<string> _playerInRoom = new List<string>();

    /// <summary>
    /// Событие кнопки Play
    /// </summary>
    public void StartPlayInGame()
    {
        // играет один
        if ((PhotonNetwork.InRoom && PhotonNetwork.PlayerList.Length == 1) || (PhotonNetwork.PlayerList.Length == 0))
        {
            PlayInSolo();
        }

        // играет не один (запускает только лидер комнаты)
        if (PhotonNetwork.PlayerList.Length > 1 && PhotonNetwork.IsMasterClient)
        {
            PlayInTeam();
        }
    }

    /// <summary>
    /// Запуск игры в одиночку
    /// </summary>
    private void PlayInSolo()
    {
        SetPlayerTeam();
        _startMode = 1;
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            GoInGame();
        }
    }

    /// <summary>
    /// Запуск игры в команде
    /// </summary>
    /// <returns></returns>
    private void PlayInTeam()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetPlayerTeam("team");
            // список userID (photon) текущих в комнате
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                _playerInRoom.Add(player.UserId);
            }


            var view = PhotonView.Get(this);
            view.RPC("JoinToMasterClientRoom", RpcTarget.Others, PhotonNetwork.MasterClient.UserId);

            _startMode = 2;
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// Устанавливает кастомный параметр команды для всех игроков в лобби
    /// </summary>
    private void SetPlayerTeam(string mode = null)
    {
        if (mode == "team")
        {
            var playerTeamIs = Random.Range(1000, 100000000).ToString();
            PhotonHashtable playerProperties = new PhotonHashtable();
            playerProperties.Add("team", playerTeamIs);
            foreach (var player in PhotonNetwork.PlayerList)
            {
                player.SetCustomProperties(playerProperties);
            }
        }
        else if(mode == null)
        {
            Player player = PhotonNetwork.LocalPlayer;
            PhotonHashtable playerProperties = new PhotonHashtable();
            playerProperties.Add("team", "None");
            player.SetCustomProperties(playerProperties);
        }
        
    }

    /// <summary>
    /// Поиск комнаты лидера
    /// </summary>
    /// <param name="masterClientID">строка userID из Photon Auth</param>
    [PunRPC]
    private void JoinToMasterClientRoom(string masterClientID)
    {
        _masterClientIDGame = masterClientID;
        FindMasterClientRoom();
    }
    

    /// <summary>
    /// Поиск комнаты лидера корутина
    /// </summary>
    /// <param name="masterClientID">строка userID из Photon Auth</param>
    /// <returns></returns>
    private void FindMasterClientRoom()
    {
        if (_masterClientIDGame != null)
        {
            _startMode = 3;
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// Обновление списка друзей, а именно, когда найден лидер в комнате присоедениться к нему
    /// </summary>
    /// <param name="friendsInfo"></param>
    public override void OnFriendListUpdate(List<FriendInfo> friendsInfo)
    {
        foreach (var friend in friendsInfo)
        {
            if (friend.UserId == _masterClientIDGame)
            {
                if (friend.IsInRoom)
                {
                    _startMode = 4;
                    GoInGame(friend.Room);
                }
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        GoInGame();
    }

    /// <summary>
    /// Последний этап работы кнопки Play
    /// </summary>
    /// <param name="roomID">Только для _startMode == 4</param>
    private void GoInGame(string roomID = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            // Play solo
            if (_startMode == 1)
            {
                gameObject.GetComponent<StartGameLevel>().enabled = true;
                PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: _roomOptions);
            }

            // Play team master client
            if (_startMode == 2)
            {
                gameObject.GetComponent<StartGameLevel>().enabled = true;
                PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: _roomOptions, typedLobby: TypedLobby.Default,
                    expectedUsers: _playerInRoom.ToArray());
            }

            // Play team NOT a master client step 1/2
            if (_startMode == 3)
            {
                PhotonNetwork.FindFriends(new string[1] {_masterClientIDGame});
            }

            // Play team NOT a master client step 2/2
            if (_startMode == 4)
            {
                gameObject.GetComponent<StartGameLevel>().enabled = true;
                PhotonNetwork.JoinRoom(roomID);
            }

            _startMode = 0;
        }
    }
}