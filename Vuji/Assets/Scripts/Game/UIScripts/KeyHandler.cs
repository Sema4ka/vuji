using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��� �����, ��� �������� �� ����������� ������� �� ���������� (��������, ����������, �������������)
/// <para>
/// AllKeys - ������ ���� KeyCode
/// </para> <para>
/// movementKeys - ������ ���� �������� ��������� � ���������
/// </para> <para>
/// abilityKeys - ������ ���� �������� ��������� � �������� ���������
/// </para> <para>
/// uiKeys - ������ ���� �������� ��������� � ���� UI
/// </para> <para>
/// keybinds - ��� ��������� �������� � KeyCode
/// </para> <para>
/// numbersKeyCodes - KeyCode ��� �������� ������ ����������
/// </para>
/// </summary>
public class KeyHandler : MonoBehaviour
{
    
    #region Fields
    public static KeyHandler instance;

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

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        KeybindManager.Binding += OnBinding;
        List<KeyCode> _allKeys = new List<KeyCode> { };
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            _allKeys.Add(keycode);
        }

        AllKeys = _allKeys;



        // Check for keybinds file
        // if haven't found one - create it with basic settings
        // string arrays contains names for 3 Categories - Movement, Abilities and UI
        uiKeys = new string[2] {"Upgrades", "EscapeMenu"};
        abilityKeys = new string[11] { "Attack", "RangeAttack", "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5", "Slot 6", "Slot 7", "Slot 8", "Slot 9"};
        movementKeys = new string[4] { "Right", "Left", "Forward", "Backward" };
        keybinds["Upgrades"] = KeyCode.U;
        keybinds["EscapeMenu"] = KeyCode.Escape;
        keybinds["Right"] = KeyCode.D;
        keybinds["Left"] =  KeyCode.A;
        keybinds["Forward"] = KeyCode.W;
        keybinds["Backward"] = KeyCode.S;
        keybinds["Attack"] = KeyCode.Space;
        keybinds["RangeAttack"] = KeyCode.Mouse0;
        for (int i = 0;i < 9; i++)
        {
            keybinds["Slot " + (i + 1).ToString()] = numbersKeyCodes[i];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (binding) return; // ���������� ������� ���������� � KeybindManager �� ����� ��������� ����������� �������
        if (paused) // ��� �������� ���� ����� ����������� ���� ������� esc
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
    #region EscapeManagementFunctions
    /// <summary>
    /// ���������� "������������" ����. �� ����� ����� ����������� ������ ������� ESC
    /// <param name="pause"></param>
    /// </summary>
    public void Pause(bool pause) {
        
        paused = pause;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>�������������� �� ����</returns>
    public bool IsPaused() {
        return paused;
    }
    /// <summary>
    /// ���������� ������ �������� ���� (��� ������� ESC ��� ���� �������������, ��������� ������� ��������� ���� �����)
    /// </summary>
    public void SetUIOpened(bool opened)
    {
        uiOpened = opened;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>������� �� ���� UI</returns>
    public bool GetUIOpened() {
        return uiOpened;
    }
    #endregion
    /// <summary>
    /// ���������� ��� ��������� ����������� ������� (�� KeybindManager)
    /// </summary>
    void OnBinding(bool isBinding)
    {
        binding = isBinding;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>������� �������� � ����������� ������</returns>
    public Dictionary<string, KeyCode> GetKeybinds()
    {
        return keybinds;
    }
    /// <summary>
    /// ����������� � �������� �������
    /// </summary>
    /// <param name="name">��������</param>
    /// <returns>����������� �������</returns>
    public KeyCode GetKeybind(string name)
    {
        if (!keybinds.ContainsKey(name)) return KeyCode.None;
        return keybinds[name];
    }
    /// <summary>
    /// ���������� ����������� ������� ��� ��������
    /// </summary>
    /// <param name="name">��������</param>
    /// <param name="key">�������</param>
    /// <returns>���� �� ����������� ��������� �������</returns>
    public bool SetKeybind(string name, KeyCode key)
    {
        if (keybinds.ContainsValue(key) && keybinds[name] != key) // ��������� ����������
        {
            return false;
        }
        keybinds[name] = key;
        return true;
    }
}
