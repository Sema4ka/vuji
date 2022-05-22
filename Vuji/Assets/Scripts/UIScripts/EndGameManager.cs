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

    // Start is called before the first frame update
    void Start()
    {
        EndGame.OnGameEnd += OnGameEnd;

    }

    void OnGameEnd(string teamName)
    {
        panel.SetActive(true);
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == teamName)
        {
            image.color = Color.blue;
            text.text = "Victory";
        }
        else
        {
            image.color = Color.red;
            text.text = "Defeat";
        }
    }


    public void LeaveGame()
    {
        SceneManager.LoadScene("Lobby");
    }
}
