using UnityEngine;

public class NoticeListController : MonoBehaviour
{
    [SerializeField] private GameObject uiNoticeList;
    [SerializeField] private Transform uiNoticeListScrollContent;
    [SerializeField] private GameObject uiNoticeInvitePrefab;

    #region Public Methods

    /// <summary>
    /// Метод открывает/скрывает список уведомлений
    /// </summary>
    public void OpenNoticeList()
    {
        uiNoticeList.SetActive(!uiNoticeList.activeSelf);
    }

    /// <summary>
    /// Медот добавляет новое уведомление в ScrollView
    /// </summary>
    /// <param name="inviteFromUserID"> userID игрока который пригласил</param>
    /// <param name="roomName">название команты, куда приглашают (Фотон комната)</param>
    public void AddInviteNotice(string inviteFromUserID, string roomName)
    {
        var noticeInvite = uiNoticeInvitePrefab.gameObject.GetComponent<NoticeInviteManager>();
        noticeInvite.usernameTextField.text = inviteFromUserID;
        noticeInvite.roomName = roomName;
        noticeInvite.lobbyManager = gameObject.GetComponent<LobbyManager>();

        var instance = Instantiate(noticeInvite.gameObject);
        instance.transform.SetParent(uiNoticeListScrollContent.transform, false);
    }

    #endregion
}