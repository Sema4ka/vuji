using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Fields

    [SerializeField] private Text username;
    [SerializeField] GameObject settings;
    private Controllers _controllers;
    // EMPTY; INLOBBY; SEARCHGAME
    public string playerStatus;
    #endregion

    #region Unity Methods

    void Start()
    {
        _controllers = GetComponent<Controllers>();
        _controllers.SetLocalUserName(username);
        // username.text = PhotonNetwork.NickName; // SET TO USERNAME
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Метод выхода из приложения
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    ///  Метод который выходит из текущего аккаунта и переключается на сцену Login
    /// </summary>
    private void LeaveAccount()
    {
        _controllers.UserOffline();
        gameObject.GetComponent<SocketServerController>().CloseConnection();
        SceneManager.LoadScene("Login");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Метод служит для включения скрипа InviteFriend и заполнения его полей
    /// </summary>
    /// <param name="invitedUserID">userID - приглашаемого</param>
    /// <param name="roomName">Имя комнаты куда нужно зайти приглашенному</param>
    public void CreateInviteFriend(int invitedUserID, string roomName = null)
    {
        var inviteFriend = gameObject.GetComponent<InviteFriend>();
        inviteFriend.enabled = true;
        inviteFriend.invitedUserID = invitedUserID;
        inviteFriend.roomName = roomName;
        inviteFriend.StartInviteFriend();
    }

    /// <summary>
    /// Метод который создает комнату и вызывает метод приглашения
    /// </summary>
    /// <param name="invitedUserID"> userID приглашаемого</param>
    public void CreateLobbyAndInviteUser(int invitedUserID)
    {
        var roomName = Random.Range(1000, 10000000).ToString();
        RoomOptions roomOptions = new RoomOptions() {IsVisible = false, PublishUserId = true};
        roomOptions.MaxPlayers = 2;
        CreateInviteFriend(invitedUserID);
        gameObject.GetComponent<LobbyManager>().playerStatus = "INLOBBY";
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    /// <summary>
    /// Метод вызывает подключение к комнате фотон
    /// </summary>
    /// <param name="roomName">название комнаты</param>
    public void AcceptInviteFriend(string roomName)
    {
        var acceptFriendInvite = gameObject.GetComponent<AcceptFriendInvite>();
        acceptFriendInvite.enabled = true;
        acceptFriendInvite.roomName = roomName;
        acceptFriendInvite.StartAcceptInvite();
    }
    
    public void ToggleSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public override void OnJoinedRoom()
    {
        if (playerStatus == "INLOBBY")
        {
            Debug.Log("YOU JOIN IN ROOM: " + PhotonNetwork.CurrentRoom.Name);
            gameObject.GetComponent<LeaveRoom>().ShowLeaveRoomButton();
        }else if (playerStatus == "SEARCHGAME")
        {
            
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("YOU LEFT ROOM");
        gameObject.GetComponent<LeaveRoom>().HideLeaveRoomButton();
        gameObject.GetComponent<StartGameLevel>().enabled = false;
        gameObject.GetComponent<PlayersFounded>().HidePlayersFounded();
    }
    
    #endregion
}