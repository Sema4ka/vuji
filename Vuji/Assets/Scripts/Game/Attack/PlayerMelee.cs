using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackTimeout = 1f;

    private Vector3 _attackPoint;
    private Vector3 _playerPosition;
    private Vector3 _mousePosition;
    private PhotonView _myView;
    private bool _isTimeout = false;

    private Animator _animator;


    private void Start()
    {
        _myView = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
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

    [PunRPC]
    public void MasterCheckMeleeAttack()
    {
        Debug.Log("Mellee check");
        if(_isTimeout) return;
        GetComponent<AnimationPlayer>().ChangePlayerAnimation_("atack_");
        StartCoroutine(AttackTiemout());

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (CanDamageThisEnemy(enemy))
            {
                int dmg = damage + gameObject.GetComponent<BaseEntity>().GetBaseDamage();
                _myView.RPC("TakeDamageRemote", RpcTarget.All, enemy.GetComponentInParent<PhotonView>().ViewID, dmg);
            }
        }
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

    private IEnumerator AttackTiemout()
    {
        _isTimeout = true;
        yield return new WaitForSeconds(attackTimeout);
        _isTimeout = false;
        GetComponent<AnimationPlayer>().ChangePlayerAnimation("iddle_");
    }
}