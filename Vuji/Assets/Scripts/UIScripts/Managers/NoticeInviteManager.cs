using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль управления списком приглашений в группу (в лобби)
/// </summary>
public class NoticeInviteManager : MonoBehaviour
{
    [SerializeField, Tooltip("Имя пользователя, пригласившего в группу")] public Text usernameTextField;
    public string roomName;
    public LobbyManager lobbyManager;

    public void AcceptInvite()
    {
        lobbyManager.AcceptInviteFriend(roomName);
        Destroy(gameObject);
    }

    public void CancelInvite()
    {
        Destroy(gameObject);
    }
}
