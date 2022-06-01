using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
/// <summary>
/// Вспомогательный модуль для отображения панели после окончания игры
/// </summary>
public class EndGameManager : MonoBehaviour
{
    [SerializeField, Tooltip("Изображение для установки спрайта победы/поражения")] Image image; // Целевое изображение 
    [SerializeField, Tooltip("Текст (Победы/поражения)")] Text text; // Целевой текст
    [SerializeField, Tooltip("Панель окончания игры")] GameObject panel; // Целевой объект панели

    [SerializeField, Tooltip("Канвас UI для уничтожения")] GameObject UiCanvas; // Объект канваса для уничтожения
    [SerializeField, Tooltip("Объект GameManager для уничтожения")] GameObject GameManager; // Объект для уничтожения

    /// <summary>
    /// Добавление функции в событие окончания игры
    /// </summary>
    void Start()
    {
        EndGame.OnGameEnd += OnGameEnd;

    }

    /// <summary>
    /// Прекращение отслеживания события
    /// </summary>
    private void OnDestroy()
    {
        EndGame.OnGameEnd -= OnGameEnd;
    }
    /// <summary>
    /// Функция для события окончаиния игры
    /// </summary>
    /// <param name="teamName">Имя победившей команды</param>
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
    /// <summary>
    /// Вспомогательная функция для кнопки выхода из игры
    /// </summary>
    public void LeaveGame()
    {
        Destroy(UiCanvas);
        Destroy(GameManager);
        SceneManager.LoadScene("Lobby");
    }
}
