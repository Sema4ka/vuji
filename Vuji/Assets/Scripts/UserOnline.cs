using UnityEngine;

public class UserOnline : MonoBehaviour
{
    private Controllers _controllers;
    private float _userTimeOnline = 10;
    private float _currentUserTimeOnline;

    #region Unity Methods

    void Start()
    {
        _controllers = gameObject.AddComponent<Controllers>();
        _currentUserTimeOnline = _userTimeOnline;
    }

    #endregion

    #region Private Methods

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