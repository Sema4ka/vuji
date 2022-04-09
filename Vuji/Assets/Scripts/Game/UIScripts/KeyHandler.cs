using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public static List<KeyCode> AllKeys;
    private bool binding = false;
#region Fields
public static Action<KeyHandler, string, KeyCode[]> keyPressed;
    #region KeyFields
    public static string[] movementKeys;
    public static string[] abilityKeys;
    public static string[] uiKeys;
    private Dictionary<string, KeyCode[]> keybinds = new Dictionary<string, KeyCode[]>();
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
        abilityKeys = new string[9] { "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5", "Slot 6", "Slot 7", "Slot 8", "Slot 9"};
        movementKeys = new string[1] { "Jump" };
        keybinds["Upgrades"] = new KeyCode[] { KeyCode.U };
        keybinds["EscapeMenu"] = new KeyCode[] { KeyCode.Escape };
        keybinds["Jump"] = new KeyCode[] {KeyCode.Space};
        for (int i = 0;i < 9; i++)
        {
            keybinds["Slot " + (i + 1).ToString()] = new KeyCode[] { numbersKeyCodes[i] };
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (binding) return;
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                keyPressed?.Invoke(this, "EscapeMenu", new KeyCode[] { KeyCode.Escape });
            }
            return;
        }
        // Read the input keys
        // Detect if some of keybings are pressed and Invoke keyPressed
        foreach (string key in keybinds.Keys)
        {
            bool toAddKey = true;
            foreach (KeyCode keyCode in keybinds[key])
            {
                if (!Input.GetKeyDown(keyCode))
                {
                    toAddKey = false;
                }
            }
            if (toAddKey && keybinds[key].Length > 0)
            {
                keyPressed?.Invoke(this, key, keybinds[key]);
            }
        }
    }
    #region EscapeManagementFunctions
    public void Pause(bool pause) {
        paused = pause;
    }

    public void SetUIOpened(bool opened)
    {
        uiOpened = opened;
    }
    public bool GetUIOpened() {
        return uiOpened;
    }
    #endregion
    void OnBinding(bool isBinding)
    {
        binding = isBinding;
    }
    public Dictionary<string, KeyCode[]> GetKeybinds()
    {
        return keybinds;
    }

    public bool SetKeybind(string name, KeyCode[] keys)
    {
        if (keybinds.ContainsValue(keys))
        {
            return false;
        }
        keybinds[name] = keys;
        return true;
    }
}
