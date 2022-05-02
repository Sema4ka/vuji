using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindManager : MonoBehaviour
{
    [SerializeField] Text keybindName;
    [SerializeField] Text keybindKeys;
    [SerializeField] KeyCode key;
    public static Action<KeybindManager, string, KeyCode> keyChanged;
    public static Action<bool> Binding;

    private bool binding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (binding)
        {
            KeyCode newKey = KeyCode.None;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                binding = false;
                keybindKeys.text = "None";
                key = KeyCode.None;
                keyChanged?.Invoke(this, keybindName.text, key);
                Binding?.Invoke(false);
                return;
            }
            foreach (KeyCode keyCode in KeyHandler.AllKeys.ToArray())
            {
                if (Input.GetKeyDown(keyCode))
                {
                    newKey = keyCode;
                }
            }
            if (newKey != KeyCode.None)
            {
                key = newKey;
                keybindKeys.text = newKey.ToString();
                keyChanged?.Invoke(this, keybindName.text, key);
                binding = false;
                Binding?.Invoke(false);
            }
        }
    }

    public string GetName()
    {
        return keybindName.text;
    }
    public KeyCode GetKey()
    {
        return key;
    }
    public void SetName(string newName)
    {
        keybindName.text = newName;
    }
    public void SetKey(KeyCode newKey)
    {
        key = newKey;
        keybindKeys.text = key.ToString();
    }

    public void SetKeybind()
    {
        binding = true;
        keybindKeys.text = "_";
        Binding?.Invoke(true);
    }

    public void ResetKeybind()
    {
        keybindKeys.text = "None";
        key = KeyCode.None;
    }
}
