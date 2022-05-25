using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject leaveRoomButton;

    /// <summary>
    /// Метод для кнопки выйти из комнаты
    /// </summary>
    public void LeaveFromRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void ShowLeaveRoomButton()
    {
        leaveRoomButton.SetActive(true);
    }
    public void HideLeaveRoomButton()
    {
        leaveRoomButton.SetActive(false);
    }
}