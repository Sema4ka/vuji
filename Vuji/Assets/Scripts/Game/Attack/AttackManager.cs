using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class AttackManager : MonoBehaviour
{
    [Header("Регулирование дистанции/радиуса атаки")]
    [Range(0.1f, 5)]
    [SerializeField] private float attackDistance = 1f;
    [Range(0.1f, 5)]
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask hitRegLayer;

    [Space(15)]

    [Header("Объекты на сущности")]
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private Transform firePoint;


    // Словарь всех проджектайлов для передачи ключа на сервер.
    [Serializable]
    public struct Projectile
    {
        public string projectileKey;
        public GameObject projectile;
    }
    [Space(15)]
    [Header("Словари ключей объектов для отправки на сервер")]
    [SerializeField] private Projectile[] projectiles;
    private Dictionary<string, GameObject> _allProjectiles = new Dictionary<string, GameObject>();

    // Словарь всех aoe для передачи ключа на сервер.
    [Serializable]
    public struct AOEstruct
    {
        public string AOEKey;
        public GameObject AOE;
    }
    [SerializeField] private AOEstruct[] AOEs;
    private Dictionary<string, GameObject> _allAOEs = new Dictionary<string, GameObject>();

    private PhotonView _myView;
    private Vector3 _playerPosition;
    private Vector3 _mousePosition;
    private Vector3 _aimDirection;
    private float _aimAngle;

    private bool _isTimeout;
  

    void Start()
    {
        _myView = gameObject.GetComponent<PhotonView>();

        // Заполнение нормального словаря всех проджектайлов
        if (projectiles.Length != 0)
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                Debug.Log(projectiles[i].projectileKey + " " + projectiles[i].projectile);
                this._allProjectiles[projectiles[i].projectileKey] = projectiles[i].projectile;
            }
        }

        // Заполнение нормального словаря всех aoe
        if (AOEs.Length != 0)
        {
            for (int i = 0; i < AOEs.Length; i++)
            {
                Debug.Log(AOEs[i].AOEKey + " " + AOEs[i].AOE);
                this._allAOEs[AOEs[i].AOEKey] = AOEs[i].AOE;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_myView.IsMine)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _playerPosition = transform.position;
            _mousePosition.z = 0;

            _aimDirection = _mousePosition - rotatePoint.position;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
            rotatePoint.rotation = Quaternion.Euler(0, 0, _aimAngle);

            // var xLen = _mousePosition.x - _playerPosition.x;
            // var yLen = _mousePosition.y - _playerPosition.y;
            // var xyLen = (float)(Math.Sqrt(Math.Pow(xLen, 2) + Math.Pow(yLen, 2)));
            // var x = (xLen * attackDistance) / xyLen + _playerPosition.x;
            // var y = (yLen * attackDistance) / xyLen + _playerPosition.y;
            // _attackPoint = new Vector3(x, y, 0);
        }
    }

    public void Attack(string attackType, int damage = 1, float attackTimeout = 1f, string projectileKey = "Fireball", string AoeKey = "AcidFloor")
    {
        switch(attackType)
        {
            case "Melee": _myView.RPC("MeleeAttack", RpcTarget.All, damage, attackTimeout); break;
            case "Projectle": _myView.RPC("ProjectileAttack", RpcTarget.All, _aimAngle, _aimDirection, projectileKey, attackTimeout); break;
            case "Aoe": _myView.RPC("AoeAttack", RpcTarget.All, AoeKey); break;
        }
    }

    [PunRPC]
    private void MeleeAttack(int damage, float attackTimeout)
    {
        Debug.Log("Mellee check");
        if(_isTimeout) return;
        StartCoroutine(AttackTiemout(attackTimeout));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, attackRadius, hitRegLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (CanDamageThisEnemy(enemy))
            {
                int dmg = damage + gameObject.GetComponent<BaseEntity>().GetBaseDamage();
                _myView.RPC("TakeDamageRemote", RpcTarget.All, enemy.GetComponentInParent<PhotonView>().ViewID, dmg);
            }
        }
    }

    [PunRPC]
    private void ProjectileAttack(float aimAngle, Vector3 aimDirection, string projectileKey)
    {
        GameObject projectile = _allProjectiles[projectileKey];
        BaseProjectile projectileBase = projectile.GetComponent<BaseProjectile>();

        projectileBase.SetAimDirection(aimDirection);
        projectileBase.SetAimAngel(aimAngle);
        projectileBase.SetSenderCollider(this.gameObject);
        projectileBase.AddDamage(gameObject.GetComponent<BaseEntity>().GetBaseDamage());
        
        GameObject projectileInst = Instantiate(projectile, firePoint.position, firePoint.rotation);
    }

    [PunRPC]
    private void AoeAttack(string AOEKey)
    {
        GameObject AOE = _allAOEs[AOEKey];
        BaseAOE aoeBase = AOE.GetComponent<BaseAOE>();
        aoeBase.SetSenderCollider(this.gameObject); 

        Instantiate(AOE, firePoint.position, Quaternion.identity);
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
