using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRange = 1f;

    private int damage = 10;
    private Vector3 _attackPoint;
    private Vector3 _playerPosition;
    private Vector3 _mousePosition;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerPosition = transform.position;
                _mousePosition.z = 0;
                var xLen = _mousePosition.x - _playerPosition.x;
                var yLen = _mousePosition.y - _playerPosition.y;
                var xyLen = (float) (Math.Sqrt(Math.Pow(xLen, 2) + Math.Pow(yLen, 2)));
                var x = (xLen * attackDistance) / xyLen + _playerPosition.x;
                var y = (yLen * attackDistance) / xyLen + _playerPosition.y;
                _attackPoint = new Vector3(x, y, 0);
                RemoteMeleeAttack(_attackPoint);
                _view.RPC("RemoteMeleeAttack", RpcTarget.All, _attackPoint);
            }
        }
    }


    [PunRPC]
    private void RemoteMeleeAttack(Vector3 attackPoint)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BaseEntity>().TakeDamage(damage);
        }
    }
}