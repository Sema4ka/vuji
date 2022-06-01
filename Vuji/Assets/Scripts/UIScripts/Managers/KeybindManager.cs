using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Модуль управления объектом настройки управления
/// </summary>
public class KeybindManager : MonoBehaviour
{
    [SerializeField, Tooltip("Текстовое поле для названия действия")] Text keybindName; // Целевое текстовое поле для названия действия
    [SerializeField, Tooltip("Текстовое поле для названия ключа действия")] Text keybindKeys; // Целевое текстовое поле для названия ключа дейсвтия
    [SerializeField, Tooltip("Ключ действия")] KeyCode key; // Ключ дейсвтвия
    public static Action<KeybindManager, string, KeyCode> keyChanged; // События изменения ключа действия
    public static Action<bool> Binding; // Событие начала изменения ключа действия

    private bool binding; // Индикатор изменения значения ключа пользователем
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
                keybindKeys.text = KeyHandler.NormalizeKeybind(newKey);
                keyChanged?.Invoke(this, keybindName.text, key);
                binding = false;
                Binding?.Invoke(false);
            }
        }
    }
    /// <summary>
    /// Получить название настройки управлеия
    /// </summary>
    /// <returns>Название настройки</returns>
    public string GetName()
    {
        return keybindName.text;
    }
    /// <summary>
    /// Получить заданный ключ настройки управления
    /// </summary>
    /// <returns>Ключ настройки управления</returns>
    public KeyCode GetKey()
    {
        return key;
    }
    /// <summary>
    /// Установить название настройки управления
    /// </summary>
    /// <param name="newName">Новое название</param>
    public void SetName(string newName)
    {
        keybindName.text = newName;
    }
    /// <summary>
    /// Установить значение ключа настройки управления
    /// </summary>
    /// <param name="newKey">Новый ключ настройки управления</param>
    public void SetKey(KeyCode newKey)
    {
        key = newKey;
        keybindKeys.text = KeyHandler.NormalizeKeybind(newKey);
    }
    /// <summary>
    /// Фунция для кпноки изменения значения ключа настройки управления
    /// </summary>
    public void SetKeybind()
    {
        binding = true;
        keybindKeys.text = "_";
        Binding?.Invoke(true);
    }
    /// <summary>
    /// Функция для обнуления значения ключа управления
    /// </summary>
    public void ResetKeybind()
    {
        keybindKeys.text = "None";
        key = KeyCode.None;
    }
}
