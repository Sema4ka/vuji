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
    public static KeyHandler instance;
    private bool spawnPause = true;
    [SerializeField] DataBase dataBase;

    public static List<KeyCode> AllKeys;
    private bool binding = false;
    
    public static Action<string, KeyCode> keyPressed;
    #region KeyFields
    public static string[] movementKeys;
    public static string[] abilityKeys;
    public static string[] uiKeys;
    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();
    #endregion
    #region ManagementFields
    private bool paused = false;
    private bool uiOpened = false;

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
     };
    #endregion
    #endregion

    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
        KeybindManager.Binding -= OnBinding;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (binding || spawnPause) return; // Считывание нажатий происходит в KeybindManager во время изменения привязанной клавиши. Не считывает во время паузы
        if (paused) // При открытии меню паузы считываются лишь нажатия esc
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
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

    void OnSpawn(GameObject player)
    {
        spawnPause = false;
    }

    #region EscapeManagementFunctions
    /// <summary>
    /// Регулирует "приостановку" игры. Во время паузы считывается только нажатие ESC
    /// <param name="pause"></param>
    /// </summary>
    public void Pause(bool pause) {
        
        paused = pause;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Приостановлена ли игра</returns>
    public bool IsPaused() {
        return paused;
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
    public bool GetUIOpened() {
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
}
