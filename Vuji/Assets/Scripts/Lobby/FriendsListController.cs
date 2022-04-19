using UnityEngine;
using UnityEngine.UI;
using StructsResponse;

public class FriendsListController : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject uiFriendsList;
    [SerializeField] private InputField friendsNameInputField;
    [SerializeField] private Transform uiFriendListScrollContent;
    [SerializeField] private GameObject uiFriendItemPrefab;
    private Controllers _controllers;

    #endregion


    #region Unity Methods

    private void Start()
    {
        _controllers = GetComponent<Controllers>();
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Метод открывает/скрывает список друзей
    /// </summary>
    public void OpenFriendsList()
    {
        if (uiFriendsList.activeSelf)
        {
            uiFriendsList.SetActive(false);
        }
        else
        {
            uiFriendsList.SetActive(true);
        }
    }

    /// <summary>
    /// Метод для поиск пользователь с похожем именем
    /// Затем в корутине будет вызван метод FillFriendsList из Controllers
    /// </summary>
    public void FindFriendsByName()
    {
        var friendsName = friendsNameInputField.text;
        _controllers.FindFriendsByName(friendsName);
    }

    /// <summary>
    /// Заполнение ScrollView префабами найденых пользователей
    /// </summary>
    /// <param name="userInfoObjects">массив всех найденных пользователей и информации о них.</param>
    public void FillFriendsList(UserInfoObject[] userInfoObjects)
    {
        // удалить всех старых пользователей
        foreach (Transform child in uiFriendListScrollContent)
        {
            Destroy(child.gameObject);
        }

        // добавить новых пользователей
        foreach (var userinfo in userInfoObjects)
        {
            // берем префаб и заполняем его поля
            var friendItem = uiFriendItemPrefab.gameObject.GetComponent<FriendItemManager>();
            friendItem.userID = userinfo.userID;
            friendItem.usernameTextField.text = userinfo.username;
            friendItem.lobbyManager = gameObject.GetComponent<LobbyManager>();

            // инициализируем поле установив его родителя как ScrollContent
            var instance = Instantiate(friendItem.gameObject);
            instance.transform.SetParent(uiFriendListScrollContent.transform, false);
        }
    }

    #endregion
}