using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text text;
    [SerializeField] GameObject panel;

    [SerializeField] GameObject UiCanvas;
    [SerializeField] GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        EndGame.OnGameEnd += OnGameEnd;

    }

    void OnGameEnd(string teamName)
    {
        panel.SetActive(true);
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name != teamName)
        {
            image.color = Color.blue;
            text.text = "Victory";
        }
        else
        {
            image.color = Color.red;
            text.text = "Defeat";
        }
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
        PhotonNetwork.LeaveRoom();
    }


    public void LeaveGame()
    {
        Destroy(UiCanvas);
        Destroy(GameManager);
        SceneManager.LoadScene("Lobby");
}
}
