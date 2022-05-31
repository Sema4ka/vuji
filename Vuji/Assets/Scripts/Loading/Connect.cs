using Photon.Pun;

public class Connect : MonoBehaviourPunCallbacks
{
    private Controllers _controllers;

    // подключение к Photon серверу
    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        _controllers = GetComponent<Controllers>();

    }
    public override void OnConnectedToMaster()
    {
        // проверка на работу fastapi сервера и авто-авторизация
        _controllers.CheckVujiServer();
    }
}