using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
/// <summary>
/// Модуль для управления меню паузы
/// </summary>
public class EscapeMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Панель меню паузы")] private RectTransform menuPanel; // Панель меню паузы
    [SerializeField, Tooltip("Панель настроек")] private RectTransform settingsPanel; // Панель настроек

    /// <summary>
    /// Добавление функции в событие нажатия ключа действия
    /// </summary>
    void Start()
    {
        KeyHandler.keyPressed += KeyPressed;
    }

    /// <summary>
    /// Отмена отслеживания события при уничтожении объекта можудя
    /// </summary>
    private void OnDestroy()
    {
        KeyHandler.keyPressed -= KeyPressed;
    }
    /// <summary>
    /// Функция для события нажатия ключа действия
    /// </summary>
    /// <param name="name"></param>
    /// <param name="key"></param>
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
    /// <summary>
    /// Вспомогательная функция для кнопки выхода из комнаты
    /// </summary>
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
    /// <summary>
    /// Вспомогательная функция для открытия настроек игры
    /// </summary>
    public void OpenSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);
        KeyHandler.instance.SetUIOpened(true);
    }
    /// <summary>
    /// Вспомогательная функция для выхода из меню паузы
    /// </summary>
    public void ResumeGame()
    {
        menuPanel.gameObject.SetActive(false);
        KeyHandler.instance.Pause(false);
    }
    /// <summary>
    /// Вспомогательная функция для выхода из игры
    /// </summary>
    public void LeaveGame()
    {
        Application.Quit();
    }
}
