using System;
using UnityEngine;
using Photon.Pun;

public class MovementPlayer : MonoBehaviour
{
    private PhotonView _view;
    private BaseEntity _player;
    private float _moveSpeed;
    private Rigidbody2D _rb2d;


    private void Start()
    {
        _player = GetComponent<BaseEntity>();
        _moveSpeed = _player.GetMoveSpeed();
        _rb2d = GetComponent<Rigidbody2D>();
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _rb2d.MovePosition(_rb2d.position + movement * _moveSpeed * Time.fixedDeltaTime);
        }
    }
}
