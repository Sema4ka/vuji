using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StopSearchGame : MonoBehaviourPunCallbacks
{
    private string _teammateID;
    private int _startMode;
    private readonly List<string> _playerInTeam = new List<string>();

    public void CancelSearchGame()
    {
        var myTeamID = PhotonNetwork.LocalPlayer.CustomProperties["team"].ToString();
        if (myTeamID != "None")
        {
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                var playerTeamID = player.CustomProperties["team"].ToString();
                if (myTeamID == playerTeamID)
                {
                    _playerInTeam.Add(player.UserId);
                    var view = PhotonView.Get(this);
                    view.RPC("LeaveFromSearch", RpcTarget.Others, player.UserId, PhotonNetwork.LocalPlayer.UserId);
                    _startMode = 2;
                    gameObject.GetComponent<LobbyManager>().playerStatus = "INLOBBY";
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
        else
        {
            gameObject.GetComponent<LobbyManager>().playerStatus = "INLOBBY";
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    private void LeaveFromSearch(string id, string teammateID)
    {
        if (PhotonNetwork.LocalPlayer.UserId == id)
        {
            _teammateID = teammateID;
            if (_teammateID != null)
            {
                _startMode = 3;
                gameObject.GetComponent<LobbyManager>().playerStatus = "INLOBBY";
                gameObject.GetComponent<StartGameLevel>().enabled = false;
                gameObject.GetComponent<PlayersFounded>().HidePlayersFounded();
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        JoinToFriendTeam();
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendsInfo)
    {
        foreach (var friend in friendsInfo)
        {
            if (friend.UserId == _teammateID)
            {
                if (friend.IsInRoom)
                {
                    _startMode = 4;
                    JoinToFriendTeam(friend.Room);
                }
            }
        }
    }

    private void JoinToFriendTeam(string roomID = null)
    {
        if (_startMode == 2)
        {
            var roomName = Random.Range(1000, 10000000).ToString();
            RoomOptions roomOptions = new RoomOptions() {IsVisible = false, PublishUserId = true};
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomName, roomOptions, expectedUsers: _playerInTeam.ToArray());
        }

        if (_startMode == 3)
        {
            PhotonNetwork.FindFriends(new string[1] {_teammateID});
        }

        if (_startMode == 4)
        {
            PhotonNetwork.JoinRoom(roomID);
        }

        _startMode = 0;
    }
}
