using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;


public class DataBase : MonoBehaviour
{
    private static string dbName = "URI=file:VujiDB.db";
    private static SqliteConnection _connection;
    private static SqliteCommand _command;
    private static IDataReader _reader;

    void Start()
    {
        CreateDB();
    }

    private static void OpenConnection()
    {
        _connection = new SqliteConnection(dbName);
        _command = new SqliteCommand(_connection);
        _connection.Open();
    }

    private static void CloseConnection()
    {
        _connection.Close();
        _command.Dispose();
    }


    private void CreateDB()
    {
        OpenConnection();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS token (id INTEGER, token STRING, PRIMARY KEY (id));";
        _command.ExecuteNonQuery();
        CloseConnection();
    }

    private bool TokenInDB()
    {
        OpenConnection();
        _command.CommandText = "SELECT * FROM token WHERE EXISTS (SELECT id FROM token WHERE id = 1)";
        _reader = _command.ExecuteReader();
        if (_reader.Read())
        {
            CloseConnection();
            return true;
        }
        else
        {
            CloseConnection();
            return false;
        }
    }

    public string get_token()
    {
        string token = "None";
        if (TokenInDB())
        {
            OpenConnection();
            _command.CommandText = "SELECT * FROM token WHERE id=1";
            _reader = _command.ExecuteReader();
            while (_reader.Read())
            {
                token = _reader["token"].ToString();
            }

            CloseConnection();
        }
        

        return token;
    }

    public void set_token(string token)
    {
        if (TokenInDB())
        {
            OpenConnection();
            _command.CommandText = "UPDATE token SET token = '" + token + "' WHERE id =1;";
            _command.ExecuteNonQuery();
            CloseConnection();
        }
        else
        {
            OpenConnection();
            _command.CommandText = "INSERT INTO token (token) VALUES ('" + token + "');";
            _command.ExecuteNonQuery();
            CloseConnection();
        }
    }
}