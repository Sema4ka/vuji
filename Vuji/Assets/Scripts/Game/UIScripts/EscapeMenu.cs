using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private KeyHandler keyHandler;

    // Start is called before the first frame update
    void Start()
    {
        KeyHandler.keyPressed += KeyPressed;
    }

    void KeyPressed(KeyHandler keyHandler, string name, KeyCode[] keys)
    {
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
        PhotonNetwork.LeaveLobby();
    }
    public void OpenSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);
        keyHandler.SetUIOpened(true);
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
