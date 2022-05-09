using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Fields

    [SerializeField] Text username;
    private Controllers _controllers;
    [SerializeField] GameObject settings;

    #endregion

    #region Unity Methods

    void Start()
    {
        _controllers = GetComponent<Controllers>();
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
    #endregion
}