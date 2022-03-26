using UnityEngine;

public class UserOnline : MonoBehaviour
{
    private Controllers _controllers;
    private DataBase _dataBase;
    private float _userTimeOnline = 10;
    private float _currentUserTimeOnline;
    
    void Start()
    {
        _controllers = gameObject.AddComponent<Controllers>();
        _dataBase = gameObject.AddComponent<DataBase>();
        _currentUserTimeOnline = _userTimeOnline;
    }


    private void Update()
    {
        _currentUserTimeOnline -= Time.deltaTime;
        if (_currentUserTimeOnline <= 0)
        {
            string token = _dataBase.GetToken();
            _controllers.UserOnline(token);
            _currentUserTimeOnline = _userTimeOnline;
        }
    }
}