using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private RectTransform settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        KeyHandler.keyPressed += KeyPressed;
    }

    void KeyPressed(string name, KeyCode key)
    {
        var keyHandler = KeyHandler.instance;
        if (name == "EscapeMenu")
        {
            if (keyHandler.GetUIOpened())
            {
                keyHandler.SetUIOpened(false);
                return;
            }
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
            keyHandler.Pause(menuPanel.gameObject.activeSelf);
        }
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void OpenSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);
        KeyHandler.instance.SetUIOpened(true);
    }
    public void ResumeGame()
    {
        menuPanel.gameObject.SetActive(false);
        KeyHandler.instance.Pause(false);
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
}
