using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private KeyHandler keyHandler;

    // Start is called before the first frame update
    void Start()
    {
        KeyHandler.keyPressed += KeyPressed;
    }

    void KeyPressed(string name, KeyCode[] keys)
    {
        if (name == "EscapeMenu")
        {
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
            keyHandler.Pause(menuPanel.gameObject.activeSelf);
        }
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
    public void OpenSettings()
    {

    }
    public void ResumeGame()
    {
        menuPanel.gameObject.SetActive(false);
        keyHandler.Pause(false);
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
}
