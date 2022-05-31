using Photon.Pun;

public class InviteFriend : MonoBehaviourPunCallbacks
{
    public int invitedUserID;
    public string roomName;
    private SocketServerController _socketServerController;

    /// <summary>
    /// Метод перед приглашением другого игрока.
    ///  Тут может быть проверка на статус приглашаемого игрока. Если игрока нет в сети не приглашать его
    /// </summary>
    public void StartInviteFriend()
    {
        if (roomName != null)
        {
            _socketServerController = GetComponent<SocketServerController>();
            _socketServerController.StartSendInviteToSocketServer(invitedUserID, roomName);
            enabled = false;
        }
    }

    public override void OnJoinedRoom()
    {
        roomName = PhotonNetwork.CurrentRoom.Name;
        StartInviteFriend();
    }
}