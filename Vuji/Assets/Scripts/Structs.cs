namespace StructsRequest
{
    [System.Serializable]
    public class LoginStructRequest
    {
        public string login;
        public string password;
    }

    [System.Serializable]
    public class RegisterStructRequest
    {
        public string login;
        public string password;
    }

    [System.Serializable]
    public class UserOnlineAndOfflineStructRequest
    {
        public string data;
    }

    [System.Serializable]
    public class FindFriendsByNameStructRequest
    {
        public string friendsName;
    }
}

namespace StructsResponse
{
    [System.Serializable]
    public class TokenStructResponse
    {
        public string token;
    }

    [System.Serializable]
    public class UserIDStructResponse
    {
        public string userID;
    }

    [System.Serializable]
    public class FindFriendsByNameStructResponse
    {
        public UserInfoObject[] friends;
    }

    [System.Serializable]
    public class UserInfoObject
    {
        public int userID;
        public string username;
    }
}

namespace ServersInfo
{
    [System.Serializable]
    public class MainServerInfo
    {
        public static string ServerDomain = "http://77.81.229.193:8000";
    }

    [System.Serializable]
    public class SocketServerInfo
    {
        public static string SocketServerIP = "77.81.229.193";
        public static int SocketServerPort = 5000;
        public static string CommandOpenConnect = "600"; // [КЛИЕНТ] подключение к серверу
        public static string CommandInvite = "700"; // [КЛИЕНТ] ответ на 601S - пользователь подключен

        public static string
            CommandHaveInvite = "701"; // [КЛИЕНТ], ответ на приглашение(принял/отказался) - не используется


        public static string CommandOpenConnectServer = "600S"; // [СЕРВЕР] отвечает на 600 запрос (хорошо/ плохо)
        public static string CommandInviteServer = "700S"; // [СЕРВЕР] отвечает на 700 запрос (хорошо/ плохо)
        public static string CommandHaveInviteServer = "701S"; // [СЕРВЕР] отправляет другому игроку приглашение
    }
}

namespace GameSettings
{
    [System.Serializable]
    public class GameSettingsOriginal
    {
        public const int MaxPlayersInGame = 4;
    }
}
