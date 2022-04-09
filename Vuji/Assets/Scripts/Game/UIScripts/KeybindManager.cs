using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindManager : MonoBehaviour
{
    [SerializeField] Text keybindName;
    [SerializeField] Text keybindKeys;
    [SerializeField] KeyCode[] keys;
    public static Action<KeybindManager, string, KeyCode[]> keyChanged;
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
            List<KeyCode> newKeys = new List<KeyCode>();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                binding = false;
                keybindKeys.text = "None";
                keys = new KeyCode[0];
                keyChanged?.Invoke(this, keybindName.text, keys);
                Binding?.Invoke(false);
                return;
            }
            foreach (KeyCode keyCode in KeyHandler.AllKeys.ToArray())
            {
                if (Input.GetKeyDown(keyCode))
                {
                    newKeys.Add(keyCode);
                }
            }
            if (newKeys.Count > 0)
            {
                int maxElems = 1;
                if (newKeys.Count > 1)
                {
                    maxElems = 2;
                }
                keybindKeys.text = String.Join(" + ", newKeys.GetRange(0, maxElems));
                keys = newKeys.GetRange(0, maxElems).ToArray();
                
                keyChanged?.Invoke(this, keybindName.text, keys);
                binding = false;
                Binding?.Invoke(false);
            }
        }
    }

    public string GetName()
    {
        return keybindName.text;
    }
    public KeyCode[] GetKeys()
    {
        return keys;
    }
    public void SetName(string newName)
    {
        keybindName.text = newName;
    }
    public void SetKeys(KeyCode[] newKeys)
    {
        keys = newKeys;
        keybindKeys.text = String.Join(" + ", newKeys);
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
    }
}
