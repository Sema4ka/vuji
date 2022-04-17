using Photon.Pun;

public class AcceptFriendInvite : MonoBehaviourPunCallbacks
{
    public string roomName;

    /// <summary>
    /// Метод на подключение к другому лобби 
    /// </summary>
    public void StartAcceptInvite()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnConnectedToMaster()
    {
        StartAcceptInvite();
    }

    public override void OnJoinedRoom()
    {
        enabled = false;
    }
}