using System.Collections;
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
    private DataBase _dataBase;
    private Controllers _controllers;
    
    void Start()
    {
        _dataBase = GetComponent<DataBase>();
        _controllers = GetComponent<Controllers>();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void RandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaveAccount()
    {
        string token = _dataBase.GetToken();
        _controllers.UserOffline(token);
        _dataBase.SetToken("");
        SceneManager.LoadScene("Login");
        
    }
}