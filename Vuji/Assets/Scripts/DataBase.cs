using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;


public class DataBase : MonoBehaviour
{
    private static string dbName = "URI=file:VujiDB.db";
    private static SqliteConnection _connection;
    private static SqliteCommand _command;
    private static IDataReader _reader;

    #region Unity Methods

    void Start()
    {
        CreateDB();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Открывает соеденение с локальной БД
    /// </summary>
    private static void OpenConnection()
    {
        _connection = new SqliteConnection(dbName);
        _command = new SqliteCommand(_connection);
        _connection.Open();
    }

    /// <summary>
    /// Закрывает соеденение с локальной БД
    /// </summary>
    private static void CloseConnection()
    {
        _connection.Close();
        _command.Dispose();
    }

    /// <summary>
    /// Метод создает локальную БД
    /// </summary>
    private void CreateDB()
    {
        OpenConnection();
        _command.CommandText = @"PRAGMA foreign_keys=OFF;
                                BEGIN TRANSACTION;
                                CREATE TABLE token (id INTEGER, token STRING, PRIMARY KEY (id));
                                CREATE TABLE keybinds (id INTEGER, name STRING, keybind STRING, category STRING, PRIMARY KEY (id));
                                INSERT INTO keybinds VALUES(0,'Slot 1','Alpha1','Ability');
                                INSERT INTO keybinds VALUES(1,'Upgrades','U','UI');
                                INSERT INTO keybinds VALUES(2,'EscapeMenu','Escape','UI');
                                INSERT INTO keybinds VALUES(3,'Right','D','Movement');
                                INSERT INTO keybinds VALUES(4,'Left','A','Movement');
                                INSERT INTO keybinds VALUES(5,'Forward','W','Movement');
                                INSERT INTO keybinds VALUES(6,'Backward','S','Movement');
                                INSERT INTO keybinds VALUES(7,'Attack','Space','Ability');
                                INSERT INTO keybinds VALUES(8,'Use Skill','Mouse0','Ability');
                                INSERT INTO keybinds VALUES(9,'Skill 1','Q','Ability');
                                INSERT INTO keybinds VALUES(10,'Skill 2','E','Ability');
                                INSERT INTO keybinds VALUES(11,'Slot 2','Alpha2','Ability');
                                INSERT INTO keybinds VALUES(13,'Slot 3','Alpha3','Ability');
                                INSERT INTO keybinds VALUES(14,'Slot 4','Alpha4','Ability');
                                INSERT INTO keybinds VALUES(15,'Slot 5','Alpha5','Ability');
                                INSERT INTO keybinds VALUES(16,'Slot 6','Alpha6','Ability');
                                INSERT INTO keybinds VALUES(17,'Slot 7','Alpha7','Ability');
                                INSERT INTO keybinds VALUES(18,'Slot 8','Alpha8','Ability');
                                INSERT INTO keybinds VALUES(19,'Slot 9','Alpha9','Ability');
                                CREATE TABLE settings (id INTEGER, name STRING, value STRING, PRIMARY KEY (id));
                                INSERT INTO settings VALUES(1,'FPS',359);
                                INSERT INTO settings VALUES(2,'VSync','True');
                                INSERT INTO settings VALUES(3,'Resolution',2);
                                INSERT INTO settings VALUES(4,'Fullscreen','False');
                                COMMIT;
";
        _command.ExecuteNonQuery();
        CloseConnection();
    }

    /// <summary>
    /// Метод проверяет наличие токена в БД
    /// </summary>
    /// <returns></returns>
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

    #endregion

    #region Public Methods

    /// <summary>
    /// Метод возвращает токен из БД
    /// </summary>
    /// <returns>токен</returns>
    public string GetToken()
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

        if (token == "")
        {
            token = "None";
        }

        return token;
    }

    /// <summary>
    /// Метод записывает новый токен в БД
    /// </summary>
    /// <param name="token">новый токен</param>
    public void SetToken(string token)
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

    public bool ExistKeybind(string name)
    {
        OpenConnection();
        _command.CommandText = "SELECT * from keybinds WHERE name ='" + name + "';";
        _reader = _command.ExecuteReader();
        if (_reader.Read())
        {
            CloseConnection();
            return true;
        }
        CloseConnection();
        return false;
        
    }

    public void AddKeybind(string name, KeyCode key, string category)
    {
        OpenConnection();
        _command.CommandText = "INSERT INTO keybinds (name, keybind, category) VALUES ('" + name + "', '" + key.ToString() + "', '" + category + "');";
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    public void SetKeybind(string name, KeyCode key)
    {
        if (!ExistKeybind(name)) { return; }
        OpenConnection();
        _command.CommandText = "UPDATE keybinds SET keybind  = '" + key.ToString() + "' WHERE name ='" + name + "';";
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    public List<Keybind> GetKeybinds()
    {
        List<Keybind> keybinds = new List<Keybind>();
        OpenConnection();
        _command.CommandText = "SELECT * from keybinds";
        _reader = _command.ExecuteReader();
        while (_reader.Read())
        {
            keybinds.Add(new Keybind(_reader["name"].ToString(), _reader["keybind"].ToString(), _reader["category"].ToString()));
        }
        CloseConnection();
        return keybinds;
    }

    public void AddSetting(string name, string value)
    {
        OpenConnection();
        _command.CommandText = "INSERT INTO settings (name, value) VALUES ('" + name + "', '" + value + "');";
        _command.ExecuteNonQuery();
        CloseConnection();
    }

    public bool ExistSetting(string name)
    {
        OpenConnection();
        _command.CommandText = "SELECT * from settings WHERE name ='" + name + "';";
        _reader = _command.ExecuteReader();
        if (_reader.Read())
        {
            CloseConnection();
            return true;
        }
        CloseConnection();
        return false;
    }

    public void SetSetting(string name, string value)
    {
        if (!ExistSetting(name)) { return; }
        OpenConnection();
        _command.CommandText = "UPDATE settings SET value  = '" + value + "' WHERE name ='" + name + "';";
        _command.ExecuteNonQuery();
        CloseConnection();
    }

    public List<Setting> GetSettings()
    {
        List<Setting> keybinds = new List<Setting>();
        OpenConnection();
        _command.CommandText = "SELECT * from settings";
        _reader = _command.ExecuteReader();
        while (_reader.Read())
        {
            keybinds.Add(new Setting(_reader["name"].ToString(), _reader["value"].ToString()));
        }
        CloseConnection();
        return keybinds;  
    }

    #endregion
}

public class Keybind
{
    public string name;
    public string key;
    public string category;

    public Keybind(string name, string key, string category)
    {
        this.name = name;
        this.key = key;
        this.category = category;
    }
}

public class Setting
{
    public string name;
    public string value;
    public Setting(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}