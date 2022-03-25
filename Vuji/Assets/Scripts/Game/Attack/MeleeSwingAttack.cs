using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeleeSwingAttack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRange = 1f;

    private int damage = 10;
    private Vector3 _attackPoint;
    private Vector3 _playerPosition;
    private Vector3 _mouseWorldPosition;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    void Update()
    {
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _playerPosition = this.transform.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (_view.IsMine)
        {
            _mouseWorldPosition.z = 0;
            var xLen = _mouseWorldPosition.x - _playerPosition.x;
            var yLen = _mouseWorldPosition.y - _playerPosition.y;
            var xyLen = (float) (Math.Sqrt(Math.Pow(xLen, 2) + Math.Pow(yLen, 2)));
            var x = (xLen * attackDistance) / xyLen + _playerPosition.x;
            var y = (yLen * attackDistance) / xyLen + _playerPosition.y;
            _attackPoint = new Vector3(x, y, 0);
            RemoteAttack(_attackPoint);
            _view.RPC("RemoteAttack", RpcTarget.All, _attackPoint);
        }
    }


    [PunRPC]
    void RemoteAttack(Vector3 attackPoint)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BaseEntity>().TakeDamage(damage);
        }
    }
}