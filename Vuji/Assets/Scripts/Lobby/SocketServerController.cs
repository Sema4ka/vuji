using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServersInfo;

public class SocketServerController : MonoBehaviour
{
    private IPEndPoint _tcpEndPoint;
    private Socket _tcpSocket;
    private Thread _thread;
    private DataBase _dataBase;
    private NoticeListController _noticeListController;
    private string _myToken;

    private void Start()
    {
        _dataBase = GetComponent<DataBase>();
        _myToken = _dataBase.GetToken();
        StartConnectToSocketServer();
        StartHearSocketServer();
        _noticeListController = gameObject.GetComponent<NoticeListController>();
    }

    #region Private Methods

    /// <summary>
    /// Метод опреедленяи полей и открытия потока на подключение к SocketServer
    /// </summary>
    private void StartConnectToSocketServer()
    {
        _tcpEndPoint = new IPEndPoint(IPAddress.Parse(SocketServerInfo.SocketServerIP),
            SocketServerInfo.SocketServerPort);
        _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ConnectToSocketServer();
    }

    /// <summary>
    ///  Метод который подключается к SocketServer и отпраляет токен для идентификации
    /// </summary>
    private void ConnectToSocketServer()
    {
        _tcpSocket.Connect(_tcpEndPoint);
        byte[] data = CreateMessageForSocketServer(SocketServerInfo.CommandOpenConnect, _myToken);
        _tcpSocket.Send(data);
    }

    /// <summary>
    /// Метод который генерирует из входных данных сообещение для SocketServer
    /// </summary>
    /// <param name="type">тип сообщения из Custom protocl</param>
    /// <param name="data">набор данных</param>
    /// <returns>байты сообщения которые нужно отправить SocketServer</returns>
    private byte[] CreateMessageForSocketServer(string type, params string[] data)
    {
        var message = type + ":";
        foreach (var info in data)
        {
            message += info + ":";
        }

        message = message.Remove(message.Length - 1);
        var messageByte = Encoding.UTF8.GetBytes(message);
        return messageByte;
    }

    /// <summary>
    /// Метод отправляющий приглашение игроку на SocketServer
    /// </summary>
    /// <param name="invitedUserID">id приглашаемого пользователя</param>
    /// <param name="roomName">название комнаты куда должен присоедениться приглашенный</param>
    private void SendInviteToSocketServer(int invitedUserID, string roomName)
    {
        if (_tcpSocket.Connected)
        {
            byte[] data = CreateMessageForSocketServer(SocketServerInfo.CommandInvite,
                _myToken,
                invitedUserID.ToString(),
                roomName);
            _tcpSocket.Send(data);
        }
    }

    /// <summary>
    /// Метод открывающий поток на прослушивание сервера
    /// </summary>
    private void StartHearSocketServer()
    {
        _thread = new Thread(HearSocketServer);
        _thread.Start();
    }

    /// <summary>
    /// Метод который слушает сервер и полученное сообщение отправляет в обработчик
    /// </summary>
    private void HearSocketServer()
    {
        try
        {
            while (_tcpSocket.Connected)
            {
                var buffer = new byte[256];
                var size = 0;
                var answ = new StringBuilder();
                do
                {
                    size = _tcpSocket.Receive(buffer);
                    answ.Append(Encoding.UTF8.GetString(buffer, 0, size));
                } while (_tcpSocket.Available > 0);

                Debug.Log("MESSAGE NEW" + answ.ToString());
                HandlerMessageFromServer(answ.ToString());
            }
        }
        catch (SocketException ex) when (ex.ErrorCode == 10004)
        {
            // эта ошибка может возникнуть когда в главном потоке отключаешься от SocketServer
        }
    }

    /// <summary>
    /// обработчик сообщений от SocketServer
    /// </summary>
    /// <param name="data">сообщение от сервера</param>
    private void HandlerMessageFromServer(string data)
    {
        string[] message = data.Split(':');
        string command = message[0];
        if (command == SocketServerInfo.CommandOpenConnectServer)
        {
            Debug.Log("You connected to server");
        }

        if (command == SocketServerInfo.CommandInviteServer)
        {
            Debug.Log("You invited a user");
        }

        if (command == SocketServerInfo.CommandHaveInviteServer)
        {
            string inviteFromUserID = message[1];
            string roomName = message[2];
            // позволяет вызывать методы из главного потока
            UnityMainThreadDispatcher.Instance()
                .Enqueue(() => _noticeListController.AddInviteNotice(inviteFromUserID, roomName));
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// открытие потока на отправку приглашения другому игроку
    /// </summary>
    /// <param name="invitedUserID">id приглашаемого пользователя</param>
    /// <param name="roomName">название комнаты куда должен присоедениться приглашенный</param>
    public void StartSendInviteToSocketServer(int invitedUserID, string roomName)
    {
        _thread = new Thread(() => { SendInviteToSocketServer(invitedUserID, roomName); });
        _thread.Start();
    }

    /// <summary>
    /// Метод отключается от SocketServer
    /// </summary>
    public void CloseConnection()
    {
        _tcpSocket.Shutdown(SocketShutdown.Both);
        _tcpSocket.Close();
    }

    #endregion
}