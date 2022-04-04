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
}