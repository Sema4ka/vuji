using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Connect : MonoBehaviourPunCallbacks
{
	private Controllers _controllers;
	// подключение к Photon серверу
    void Start()
    {
	    _controllers = GetComponent<Controllers>();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
	    // проверка на работу fastapi сервера и авто-авторизация
	    _controllers.CheckVujiServer();
    }
    
}
