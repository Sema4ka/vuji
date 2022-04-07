using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public static Action<string, KeyCode[]> keyPressed;
    public static string[] movementKeys;
    public static string[] abilityKeys;
    public static string[] uiKeys;
    private Dictionary<string, KeyCode[]> keybinds = new Dictionary<string, KeyCode[]>();
    private bool paused = false;

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
    // Start is called before the first frame update
    void Start()
    {

        // Check for keybinds file
        // if haven't found one - create it with basic settings
        // string arrays contains names for 3 Categories - Movement, Abilities and UI
        uiKeys = new string[2];
        uiKeys[0] = "Upgrades";
        uiKeys[1] = "EscapeMenu";
        abilityKeys = new string[9] { "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5", "Slot 6", "Slot 7", "Slot 8", "Slot 9"};
        keybinds["Upgrades"] = new KeyCode[] { KeyCode.U };
        keybinds["EscapeMenu"] = new KeyCode[] { KeyCode.Escape };
        for (int i = 0;i < 9; i++)
        {
            keybinds["Slot " + (i + 1).ToString()] = new KeyCode[] { numbersKeyCodes[i] };
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                keyPressed?.Invoke("EscapeMenu", new KeyCode[] { KeyCode.Escape });
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
            if (toAddKey)
            {
                keyPressed?.Invoke(key, keybinds[key]);
            }
        }
    }

    public void Pause(bool pause) {
        paused = pause;
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
