using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // я определяю объекты в Unity, но можно и конструкции Find, GetComponent
    private Controllers _controllers;
    public InputField loginInput;
    public InputField passwordInput;

    void Start()
    {
        Screen.SetResolution(1054, 593, false);
        _controllers = GetComponent<Controllers>();
    }

    /// <summary>
    /// Авторизация в аккаунт
    /// </summary>
    public void LoginInAccount()
    {
        _controllers.Login(loginInput.text, passwordInput.text);
    }

    public void ShowRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }
}