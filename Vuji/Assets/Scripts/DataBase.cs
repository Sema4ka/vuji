using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

/// <summary>
/// Модуль взаимодействия с базой данных игры
/// </summary>
public class DataBase : MonoBehaviour
{
    private static string dbName = "URI=file:VujiDB.db"; // Путь к файлу бд
    private static SqliteConnection _connection; // Вспомогательное поле для хранения сессии
    private static SqliteCommand _command; // Вспомогательное поле для хранения команды (запроса) SQL
    private static IDataReader _reader; // Вспомогательное поля для чтеня даннных 

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
                                INSERT INTO keybinds VALUES(1,'EscapeMenu','Escape','UI');
                                INSERT INTO keybinds VALUES(2,'Right','D','Movement');
                                INSERT INTO keybinds VALUES(3,'Left','A','Movement');
                                INSERT INTO keybinds VALUES(4,'Forward','W','Movement');
                                INSERT INTO keybinds VALUES(5,'Backward','S','Movement');
                                INSERT INTO keybinds VALUES(6,'Attack','Space','Ability');
                                INSERT INTO keybinds VALUES(7,'Use Skill','Mouse0','Ability');
                                INSERT INTO keybinds VALUES(8,'Skill 1','Q','Ability');
                                INSERT INTO keybinds VALUES(9,'Skill 2','E','Ability');
                                INSERT INTO keybinds VALUES(10,'Slot 1','Alpha1','Ability');
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
                                INSERT INTO settings VALUES(5,'General volume','1');
                                INSERT INTO settings VALUES(6,'Music volume','1');
                                INSERT INTO settings VALUES(7,'Environment volume','1');
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
    /// <summary>
    /// Проверяет, существует ли в базе данных настройка управления по названию действия
    /// </summary>
    /// <param name="name">Название действия</param>
    /// <returns>Существует ли настройка в базе данных</returns>
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
    /// <summary>
    /// Добавляет настройку управления в базу данных с указанными параметрами
    /// </summary>
    /// <param name="name">Название действия</param>
    /// <param name="key">Ключ действия</param>
    /// <param name="category">Категория действия ("Movement", "Ability", "UI")</param>
    public void AddKeybind(string name, KeyCode key, string category)
    {
        OpenConnection();
        _command.CommandText = "INSERT INTO keybinds (name, keybind, category) VALUES ('" + name + "', '" + key.ToString() + "', '" + category + "');";
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    /// <summary>
    /// Установить ключ действия в базе данных по названию
    /// </summary>
    /// <param name="name">Название действия</param>
    /// <param name="key">Новый ключ действия</param>
    public void SetKeybind(string name, KeyCode key)
    {
        if (!ExistKeybind(name)) { return; }
        OpenConnection();
        _command.CommandText = "UPDATE keybinds SET keybind  = '" + key.ToString() + "' WHERE name ='" + name + "';";
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    /// <summary>
    /// Получить список настоек управления из базы данных
    /// </summary>
    /// <returns>Список настроек управления</returns>
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
    /// <summary>
    /// Добавить настроку в базу данных с указанными параметрами
    /// </summary>
    /// <param name="name">Название настройки</param>
    /// <param name="value">Значение настройки</param>
    public void AddSetting(string name, string value)
    {
        OpenConnection();
        _command.CommandText = "INSERT INTO settings (name, value) VALUES ('" + name + "', '" + value + "');";
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    /// <summary>
    /// Проверить, существует ли настройка с указаным именем в базе данных
    /// </summary>
    /// <param name="name">Имя настройки</param>
    /// <returns>Существует ли настройка</returns>
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
    /// <summary>
    /// Установить значение для настройки с указанным именем
    /// </summary>
    /// <param name="name">Название настройки</param>
    /// <param name="value">Новое значение</param>
    public void SetSetting(string name, string value)
    {
        if (!ExistSetting(name)) { return; }
        OpenConnection();
        _command.CommandText = "UPDATE settings SET value  = '" + value + "' WHERE name ='" + name + "';";
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    /// <summary>
    /// Получить список настроек из базы данных
    /// </summary>
    /// <returns>Список настроек</returns>
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

/// <summary>
/// Вспомогательный класс для передачи информации о сохраненных настройках управления
/// </summary>
public class Keybind
{
    public string name; // Название действия
    public string key; // Ключ действия
    public string category; // Категория действия
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="name">Название действия</param>
    /// <param name="key">Ключ действия</param>
    /// <param name="category">Категория действия</param>
    public Keybind(string name, string key, string category)
    {
        this.name = name;
        this.key = key;
        this.category = category;
    }
}
/// <summary>
/// Вспомогательный класс для передачи сохраненных настроек
/// </summary>
public class Setting
{
    public string name; // Название настройки
    public string value; // Значение
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="name">Название</param>
    /// <param name="value">Значение</param>
    public Setting(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}