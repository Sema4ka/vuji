using System.Collections;
using StructsRequest;
using StructsResponse;
using ServersInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;



public class Controllers : MonoBehaviour
{
    private readonly string _serverDomain = MainServerInfo.ServerDomain;
    private DataBase _dataBase;

    #region Unity Methods

    private void Start()
    {
        _dataBase = gameObject.GetComponent<DataBase>();
    }

    #endregion

    #region Public Methods

    public void CheckVujiServer()
    {
        StartCoroutine(CheckVujiServerNet());
    }

    public void Login(string login, string password)
    {
        StartCoroutine(LoginNet(login, password));
    }

    public void Register(string login, string password)
    {
        StartCoroutine(RegisterNet(login, password));
    }

    public void UserOnline()
    {
        string token = _dataBase.GetToken();
        StartCoroutine(UserOnlineNet(token));
    }

    public void UserOffline()
    {
        string token = _dataBase.GetToken();
        _dataBase.SetToken("");
        StartCoroutine(UserOfflineNet(token));
    }

    public void GetUserID(string token)
    {
        StartCoroutine(GetUserIDNet(token));
    }

    public void FindFriendsByName(string friendsName)
    {
        string token = _dataBase.GetToken();
        StartCoroutine(FindFriendsByNameNet(token, friendsName));
    }

    public void SetLocalUserName(Text field)
    {
        if (_dataBase == null) _dataBase = gameObject.GetComponent<DataBase>(); ;
        string token = _dataBase.GetToken();
        StartCoroutine(GetUserInfo(token, field));
        
        
    }

    #endregion

    #region Private IEnumerator Methods

    private void AutoAuth()
    {
        StartCoroutine(AutoAuthNet());
    }

    private IEnumerator AutoAuthNet()
    {
        WWWForm form = new WWWForm();
        string token = _dataBase.GetToken();

        UnityWebRequest www = UnityWebRequest.Post(_serverDomain + "/auth", form);
        www.SetRequestHeader("Authorization", token);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            SceneManager.LoadScene("Login");
        }
        else
        {
            TokenStructResponse tokenStructResponse =
                JsonUtility.FromJson<TokenStructResponse>(www.downloadHandler.text);
            _dataBase.SetToken(tokenStructResponse.token);
            SceneManager.LoadScene("Lobby");
        }
    }

    private IEnumerator CheckVujiServerNet()
    {
        UnityWebRequest www = UnityWebRequest.Get(_serverDomain);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Vuji server: [OFFLINE]");
        }
        else
        {
            Debug.Log("Vuji server: [ONLINE]");
            AutoAuth();
        }
    }

    private IEnumerator LoginNet(string login, string password)
    {
        WWWForm form = new WWWForm();

        LoginStructRequest loginStruct = new LoginStructRequest();
        loginStruct.login = login;
        loginStruct.password = password;
        string json = JsonUtility.ToJson(loginStruct);
        UnityWebRequest www = UnityWebRequest.Post(_serverDomain + "/login", form);
        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(json);
        UploadHandler uploadHandler = new UploadHandlerRaw(postBytes);

        www.uploadHandler = uploadHandler;
        www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");


        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Incorrect fields");
        }
        else
        {
            TokenStructResponse tokenStructResponse =
                JsonUtility.FromJson<TokenStructResponse>(www.downloadHandler.text);
            _dataBase.SetToken(tokenStructResponse.token);
            SceneManager.LoadScene("Lobby");
        }
    }

    private IEnumerator RegisterNet(string login, string password)
    {
        WWWForm form = new WWWForm();

        RegisterStructRequest registerStructRequest = new RegisterStructRequest();
        registerStructRequest.login = login;
        registerStructRequest.password = password;

        string json = JsonUtility.ToJson(registerStructRequest);
        UnityWebRequest www = UnityWebRequest.Post(_serverDomain + "/register", form);
        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(json);
        UploadHandler uploadHandler = new UploadHandlerRaw(postBytes);

        www.uploadHandler = uploadHandler;
        www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");


        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Login already used");
        }
        else
        {
            TokenStructResponse tokenStructResponse =
                JsonUtility.FromJson<TokenStructResponse>(www.downloadHandler.text);
            _dataBase.SetToken(tokenStructResponse.token);
            SceneManager.LoadScene("Login");
        }
    }

    private IEnumerator UserOnlineNet(string token)
    {
        UserOnlineAndOfflineStructRequest userStruct = new UserOnlineAndOfflineStructRequest();
        userStruct.data = "None";
        string json = JsonUtility.ToJson(userStruct);
        UnityWebRequest www = UnityWebRequest.Put(_serverDomain + "/user_online", json);
        www.SetRequestHeader("Authorization", token);
        www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        yield return www.SendWebRequest();
    }

    private IEnumerator UserOfflineNet(string token)
    {
        UserOnlineAndOfflineStructRequest userStruct = new UserOnlineAndOfflineStructRequest();
        userStruct.data = "None";
        string json = JsonUtility.ToJson(userStruct);
        UnityWebRequest www = UnityWebRequest.Put(_serverDomain + "/user_offline", json);
        www.SetRequestHeader("Authorization", token);
        www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        yield return www.SendWebRequest();
    }

    private IEnumerator GetUserIDNet(string token)
    {
        UnityWebRequest www = UnityWebRequest.Get(_serverDomain + "/user_id");
        www.SetRequestHeader("Authorization", token);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ERROR: cant set user_id in Photon.NickMame");
        }
        else
        {
            UserIDStructResponse userIDStructResponse =
                JsonUtility.FromJson<UserIDStructResponse>(www.downloadHandler.text);
            string userID = userIDStructResponse.userID;
        }
    }

    private IEnumerator FindFriendsByNameNet(string token, string friendsName)
    {
        WWWForm form = new WWWForm();
        FindFriendsByNameStructRequest findFriendsByNameStructRequest = new FindFriendsByNameStructRequest();
        findFriendsByNameStructRequest.friendsName = friendsName;

        string json = JsonUtility.ToJson(findFriendsByNameStructRequest);
        UnityWebRequest www = UnityWebRequest.Post(_serverDomain + "/find_friends_by_name", form);
        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(json);
        UploadHandler uploadHandler = new UploadHandlerRaw(postBytes);

        www.uploadHandler = uploadHandler;
        www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        www.SetRequestHeader("Authorization", token);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Unexpected error findFriends");
        }
        else
        {
            FindFriendsByNameStructResponse findFriendsByNameStructResponse =
                JsonUtility.FromJson<FindFriendsByNameStructResponse>("{\"friends\":" + www.downloadHandler.text + "}");
            UserInfoObject[] userInfoObject = findFriendsByNameStructResponse.friends;

            FriendsListController friendsListController = gameObject.GetComponent<FriendsListController>();
            friendsListController.FillFriendsList(userInfoObject);
        }
    }

    private IEnumerator GetUserInfo(string token, Text field=null)
    {
        UserOnlineAndOfflineStructRequest userStruct = new UserOnlineAndOfflineStructRequest();
        userStruct.data = "None";
        string json = JsonUtility.ToJson(userStruct);
        UnityWebRequest www = UnityWebRequest.Get(_serverDomain + "/me");
        www.SetRequestHeader("Authorization", token);
        www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ERROR: cant get username. " + www.responseCode);
        }
        else
        {
            UserInfoStructResponse response=
                JsonUtility.FromJson<UserInfoStructResponse>(www.downloadHandler.text);
            if (field != null) field.text = response.login;
            
        }
    }

    #endregion
}