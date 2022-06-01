using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// Модуль управления объектом пользователя, доступного для приглашения в группу (в лобби)
/// </summary>
public class FriendItemManager : MonoBehaviourPunCallbacks
{
    #region Fields

    public int userID; // ID целевого игрока
    public LobbyManager lobbyManager; // Модуль лобби
    [SerializeField, Tooltip("Текстовое поле для имени пользователя")] public Text usernameTextField; // Целевое текстовое поле для имени пользователя

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