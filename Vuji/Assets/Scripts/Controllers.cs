using System;
using System.Collections;
using StructsRequest;
using StructsResponse;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Controllers : MonoBehaviour
{
    private string SERVER_DOMAIN = "http://127.0.0.1:8000";
    private DataBase _dataBase;

    private void Start()
    {
        _dataBase = gameObject.AddComponent<DataBase>();
    }

    public void AutoAuth()
    {
        StartCoroutine(AutoAuthNet());
    }

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

    private IEnumerator AutoAuthNet()
    {
        WWWForm form = new WWWForm();
        string token = _dataBase.get_token();
        
        // чтобы была возможность войти в разные аккаунты
        token = "TESTTOKEN";
        
        UnityWebRequest www = UnityWebRequest.Post(SERVER_DOMAIN + "/auth", form);
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
            _dataBase.set_token(tokenStructResponse.token);
            SceneManager.LoadScene("Lobby");
        }
    }

    private IEnumerator CheckVujiServerNet()
    {
        UnityWebRequest www = UnityWebRequest.Get(SERVER_DOMAIN);
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
        UnityWebRequest www = UnityWebRequest.Post(SERVER_DOMAIN + "/login", form);
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
            _dataBase.set_token(tokenStructResponse.token);
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
        UnityWebRequest www = UnityWebRequest.Post(SERVER_DOMAIN + "/register", form);
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
            _dataBase.set_token(tokenStructResponse.token);
            SceneManager.LoadScene("Login");
        }
    }
}