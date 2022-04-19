using UnityEngine;

public class UserOnline : MonoBehaviour
{
    private Controllers _controllers;
    private float _userTimeOnline = 5;
    private float _currentUserTimeOnline;

    #region Unity Methods

    void Start()
    {
        _controllers = gameObject.AddComponent<Controllers>();
        _currentUserTimeOnline = _userTimeOnline;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// каждые {_userTimeOnline} секунд отправляет сообщение на Server(FastAPI), о том что пользователь по токену в сети 
    /// </summary>
    private void Update()
    {
        _currentUserTimeOnline -= Time.deltaTime;
        if (_currentUserTimeOnline <= 0)
        {
            _controllers.UserOnline();
            _currentUserTimeOnline = _userTimeOnline;
        }
    }

    #endregion
}