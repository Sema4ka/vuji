using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    private Controllers _controllers;
    private DataBase _dataBase;
    #region Unity Methods

    void Start()
    {
        _controllers = GetComponent<Controllers>();
        _dataBase = GetComponent<DataBase>();
        string token = _dataBase.GetToken();
        _controllers.GetUserID(token);
    }

    #endregion

    #region Private Methods

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    private void RandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void LeaveAccount()
    {
        _controllers.UserOffline();
        SceneManager.LoadScene("Login");
    }

    #endregion

    #region Public Methods

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GenerateTest");
    }

    #endregion
}
