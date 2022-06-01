using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Модуль для всего, что связанно со считыванием нажатий на клавиатуру (привязка, считывание, регулирование)
/// <para>
/// AllKeys - Список всех KeyCode
/// </para> <para>
/// movementKeys - Список всех действий связанных с движением
/// </para> <para>
/// abilityKeys - Список всех действий связанных с умениями персонажа
/// </para> <para>
/// uiKeys - Список всех действий связанных с меню UI
/// </para> <para>
/// keybinds - Все связанные действия и KeyCode
/// </para> <para>
/// numbersKeyCodes - KeyCode для цифровых клавиш клавиатуры
/// </para>
/// </summary>
public class KeyHandler : MonoBehaviour
{

    #region Fields
    public static KeyHandler instance; // Объект класса, необходим для обновления  и синхронизации
    private bool spawnPause = true; // Индикатор паузы до и после игры
    [SerializeField] DataBase dataBase; // База данныъ
    public static List<KeyCode> AllKeys; // Список всех ключей, доступных для считывания
    private bool binding = false; // Индикатор паузы при изменении ключа дейсвия

    public static Action<string, KeyCode> keyPressed; // Событие нажатия пользователем ключей действий
    #region KeyFields
    public static string[] movementKeys; // Список ключей, связанных с движением
    public static string[] abilityKeys; // Список ключей, связанных с способностями персонажа
    public static string[] uiKeys; // Список ключей, связанных с пользовательским интерфейсом
    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>(); // Спиоск всех считываемых ключей действий
    #endregion
    #region ManagementFields
    private bool paused = false; // Индикатор паузы (для меню паузы)
    private bool uiOpened = false; // Индикатор открытия панелей UI (для меню паузы)

    public static KeyCode[] numbersKeyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     }; // Список всех ключей цифр
    #endregion
    #endregion
    /// <summary>
    /// Убирает функции из событий, дабы они не срабатывали после уничтожения объекта модуля
    /// </summary>
    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
        KeybindManager.Binding -= OnBinding;
    }

    /// <summary>
    /// Добавление функций в события, считывание настроек управления и заполнение списков.
    /// </summary>
    void Start()
    {
        Debug.Log("KeyHandler Started");
        instance = this;
        KeybindManager.Binding += OnBinding;
        SpawnPlayers.OnSpawn += OnSpawn;
        List<KeyCode> _allKeys = new List<KeyCode> { };
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            _allKeys.Add(keycode);
        }

        AllKeys = _allKeys;

        List<Keybind> keybindList = dataBase.GetKeybinds();

        List<string> movementKeysList = new List<string>();
        List<string> abilityKeysList = new List<string>();
        List<string> uiKeysList = new List<string>();

        foreach (Keybind keybind in keybindList)
        {
            KeyCode thisKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), keybind.key);
            keybinds[keybind.name] = thisKeyCode;
            switch (keybind.category)
            {
                case "Movement":
                    movementKeysList.Add(keybind.name);
                    break;
                case "Ability":
                    abilityKeysList.Add(keybind.name);
                    break;
                case "UI":
                    uiKeysList.Add(keybind.name);
                    break;
                default:
                    Debug.Log("Keybind without a category: " + keybind.name);
                    break;
            }
        }

        movementKeys = movementKeysList.ToArray();
        abilityKeys = abilityKeysList.ToArray();
        uiKeys = uiKeysList.ToArray();

    }

    /// <summary>
    /// Считывание нажатых ключей, проверка паузы
    /// </summary>
    void Update()
    {
        if (binding || spawnPause) return; // Считывание нажатий происходит в KeybindManager во время изменения привязанной клавиши. Не считывает во время паузы
        if (paused) // При открытии меню паузы считываются лишь нажатия esc
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                keyPressed?.Invoke("EscapeMenu", KeyCode.Escape);
            }
            return;
        }
        // Read the input keys
        // Detect if some of keybings are pressed and Invoke keyPressed
        foreach (string key in keybinds.Keys)
        {
            if (Input.GetKeyDown(keybinds[key]) && !movementKeys.Contains(key))
            {
                keyPressed?.Invoke(key, keybinds[key]);
            }
        }
    }
    /// <summary>
    /// Функция для события появления локального игрока
    /// </summary>
    /// <param name="player">Объект игрока</param>
    void OnSpawn(GameObject player)
    {
        spawnPause = false;
    }

    #region EscapeManagementFunctions
    /// <summary>
    /// Регулирует "приостановку" игры. Во время паузы считывается только нажатие ESC
    /// <param name="pause"></param>
    /// </summary>
    public void Pause(bool pause)
    {

        paused = pause;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Приостановлена ли игра</returns>
    public bool IsPaused()
    {
        return paused;
    }
    /// <summary>
    /// Индикатор того, что игра уже началась и не приостановлена, а также не открыты никакие окна UI
    /// </summary>
    /// <returns></returns>
    public bool IsClearAndPlaying()
    {
        return !paused && !uiOpened && !spawnPause && !binding;
    }
    /// <summary>
    /// Регулирует статус открытых меню (при нажатии ESC все меню сворачиваются, повтроное нажатие открывает меню паузы)
    /// </summary>
    public void SetUIOpened(bool opened)
    {
        uiOpened = opened;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Открыто ли меню UI</returns>
    public bool GetUIOpened()
    {
        return uiOpened;
    }
    #endregion
    /// <summary>
    /// Вызывается при изменении привязанной клавиши (из KeybindManager)
    /// </summary>
    void OnBinding(bool isBinding)
    {
        binding = isBinding;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Словарь действий и привязанных клавиш</returns>
    public Dictionary<string, KeyCode> GetKeybinds()
    {
        return keybinds;
    }
    /// <summary>
    /// Привязанная к действию клавиша
    /// </summary>
    /// <param name="name">Действие</param>
    /// <returns>Привязанная клавиша</returns>
    public KeyCode GetKeybind(string name)
    {
        if (!keybinds.ContainsKey(name)) return KeyCode.None;
        return keybinds[name];
    }
    /// <summary>-*
    /// Установить привязанную клавишу для действия
    /// </summary>
    /// <param name="name">Действие</param>
    /// <param name="key">Клавиша</param>
    /// <returns>Была ли установлена указанная клавиша</returns>
    public bool SetKeybind(string name, KeyCode key)
    {
        if (keybinds.ContainsValue(key) && keybinds[name] != key) // Исключить повторения
        {
            return false;
        }
        keybinds[name] = key;
        dataBase.SetKeybind(name, key);
        return true;
    }
    /// <summary>
    /// Вспомогательная функция для преобразования названия ключа в более логичное
    /// </summary>
    /// <param name="code">Ключ действия</param>
    /// <returns>Преобразованное название ключв</returns>
    public static string NormalizeKeybind(KeyCode code)
    {
        string keyName = code.ToString();
        if (keyName.StartsWith("Alpha"))
        {
            return keyName[keyName.Length - 1].ToString();
        }
        else if (keyName.StartsWith("Mouse"))
        {
            switch (keyName)
            {
                case "Mouse0":
                    return "LMB";
                case "Mouse1":
                    return "RMB";
                case "Mouse2":
                    return "MMB";
                case "Mouse3":
                    return "SMB 1";
                case "Mouse4":
                    return "SMB 2";
                case "Mouse5":
                    return "SMB 3";
                case "Mouse6":
                    return "SMB 4";
            }
        }
        return keyName;
    }
}
