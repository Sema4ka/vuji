using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;
public class PlayersFounded : MonoBehaviour
{
    [SerializeField] private GameObject playersFounded;
    [SerializeField] private Text playersFoundedText;
    
    /// <summary>
    /// Обновляет текустовую информацию о текущем кол-ве игроков
    /// </summary>
    public void UpdatePlayersFounded()
    {
        playersFoundedText.text = PhotonNetwork.PlayerList.Length + " / " + GameSettingsOriginal.MaxPlayersInGame + " founded";
    }

    /// <summary>
    /// Включает объект "PlayersFounded"
    /// </summary>
    public void ShowPlayersFounded()
    {
        playersFounded.SetActive(true);
    }

    /// <summary>
    /// Выключает объект "PlayersFounded"
    /// </summary>
    public void HidePlayersFounded()
    {
        playersFounded.SetActive(false);
    }
}
