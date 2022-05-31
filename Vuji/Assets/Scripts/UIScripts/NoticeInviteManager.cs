using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeInviteManager : MonoBehaviour
{
    [SerializeField] public Text usernameTextField;
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
