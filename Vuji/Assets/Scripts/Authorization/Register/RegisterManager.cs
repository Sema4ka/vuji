using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    private Controllers _controllers;
    public InputField loginInput;
    public InputField passwordOneInput;
    public InputField passwordTwoInput;

    void Start()
    {
        _controllers = GetComponent<Controllers>();
    }

    /// <summary>
    /// Регистрация аккаунта + проверка корректности заполненных полей
    /// </summary>
    public void RegisterAccount()
    {
        if (passwordOneInput.text != passwordTwoInput.text)
        {
            Debug.Log("password error");
            return;
        }
        _controllers.Register(loginInput.text, passwordOneInput.text);
    }

    public void ShowLoginScene()
    {
        SceneManager.LoadScene("Login");
    }
}