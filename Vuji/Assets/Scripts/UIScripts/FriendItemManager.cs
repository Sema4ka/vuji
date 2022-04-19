using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class FriendItemManager : MonoBehaviourPunCallbacks
{
    #region Fields

    public int userID;
    public LobbyManager lobbyManager;
    [SerializeField] public Text usernameTextField;

    #endregion
    

    #region Public Methods

    /// <summary>
    /// Метод приглашение игрока. Если текущей пользователь не в комнате то создать ее иначе пригласить игрока
    /// </summary>
    public void InviteFriend()
    {
        if (PhotonNetwork.InRoom)
        {
            lobbyManager.CreateInviteFriend(userID, PhotonNetwork.CurrentRoom.Name);
        }
        else
        {
            lobbyManager.CreateLobbyAndInviteUser(userID);
        }
    }

    #endregion
    
}