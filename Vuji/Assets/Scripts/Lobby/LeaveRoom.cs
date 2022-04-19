using Photon.Pun;
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

    public override void OnConnectedToMaster()
    {
        Debug.Log("YOU LEFT ROOM");
        leaveRoomButton.SetActive(false);
        gameObject.GetComponent<StartGameLevel>().enabled = false;
        gameObject.GetComponent<PlayersFounded>().HidePlayersFounded();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("YOU JOIN IN ROOM: " + PhotonNetwork.CurrentRoom.Name);
        leaveRoomButton.SetActive(true);
    }
}