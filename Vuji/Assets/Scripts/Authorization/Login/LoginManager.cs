using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // я определяю объекты в Unity, но можно и конструкции Find и GetComponent
    private Controllers _controllers;
    public InputField loginInput;
    public InputField passwordInput;

    void Start()
    {
        _controllers = GetComponent<Controllers>();
    }


    public void LoginInAccount()
    {
        _controllers.Login(loginInput.text, passwordInput.text);
    }

    public void ShowRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }
}