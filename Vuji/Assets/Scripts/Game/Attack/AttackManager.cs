using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask hitRegLayer;

    private Vector3 _attackPoint;
    private PhotonView _myView;
    private Vector3 _playerPosition;
    private Vector3 _mousePosition;

    private bool _isTimeout;

    void Start()
    {
        _myView = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_myView.IsMine)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _playerPosition = transform.position;
            _mousePosition.z = 0;
            var xLen = _mousePosition.x - _playerPosition.x;
            var yLen = _mousePosition.y - _playerPosition.y;
            var xyLen = (float)(Math.Sqrt(Math.Pow(xLen, 2) + Math.Pow(yLen, 2)));
            var x = (xLen * attackDistance) / xyLen + _playerPosition.x;
            var y = (yLen * attackDistance) / xyLen + _playerPosition.y;
            _attackPoint = new Vector3(x, y, 0);
        }
    }

    public void Attack(string attackType, int damage = 1, float attackTimeout = 1f)
    {
        switch(attackType)
        {
            case "Melee": _myView.RPC("MeleeAttack", RpcTarget.All, damage, attackTimeout); break;
            case "Projectle": ProjectileAttack(); break;
            case "Aoe": AoeAttack(); break;
        }

        if(attackType == "Projectile")
        {
            ProjectileAttack();
        }
    }

    [PunRPC]
    private void MeleeAttack(int damage, float attackTimeout)
    {
        Debug.Log("Mellee check");
        if(_isTimeout) return;
        StartCoroutine(AttackTiemout(attackTimeout));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint, attackRadius, hitRegLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (CanDamageThisEnemy(enemy))
            {
                int dmg = damage + gameObject.GetComponent<BaseEntity>().GetBaseDamage();
                _myView.RPC("TakeDamageRemote", RpcTarget.All, enemy.GetComponentInParent<PhotonView>().ViewID, dmg);
            }
        }
    }

    private void ProjectileAttack()
    {

    }

    private void AoeAttack()
    {
        
    }

    private bool CanDamageThisEnemy(Collider2D enemyCollider)
    {
        GameObject enemyGameObject = enemyCollider.transform.parent.gameObject;
        
        if (enemyGameObject == gameObject)
        {
            return false;
        }

        if (enemyGameObject.CompareTag("Player"))
        {
            var otherPlayerView = enemyGameObject.GetComponent<PhotonView>();

            if (otherPlayerView.Owner.GetPhotonTeam().Name == _myView.Owner.GetPhotonTeam().Name)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator AttackTiemout(float attackTimeout)
    {
        _isTimeout = true;
        yield return new WaitForSeconds(attackTimeout);
        _isTimeout = false;
    }

}
