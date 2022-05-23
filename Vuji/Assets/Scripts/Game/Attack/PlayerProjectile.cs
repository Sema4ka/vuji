using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerProjectile : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private Transform firePoint;


    [Serializable]
    public struct Projectile
    {
        public string projectileKey;
        public GameObject projectile;
    }
    [SerializeField] private Projectile[] projectiles;
    private Dictionary<string, GameObject> _allProjectiles = new Dictionary<string, GameObject>();

    private Vector3 _mousePosition;
    private Vector3 _aimDirection;
    private float _aimAngle;
    private PhotonView _view;

    #endregion

    private void Start()
    {
        _view = GetComponent<PhotonView>();

        // Заполнение нормального словаря всех проджектайлов
        if (projectiles.Length != 0)
            for (int i = 0; i < projectiles.Length; i++)
            {
                Debug.Log(projectiles[i].projectileKey + " " + projectiles[i].projectile);
                this._allProjectiles[projectiles[i].projectileKey] = projectiles[i].projectile;
            }
    }

    public void Attack(string projectileKey)
    {
        //RemoteProjectileAttack(_aimAngle, _aimDirection, projectileKey);
        _view.RPC("RemoteProjectileAttack", RpcTarget.All, _aimAngle, _aimDirection, projectileKey);
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _aimDirection = _mousePosition - rotatePoint.position;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
            rotatePoint.rotation = Quaternion.Euler(0, 0, _aimAngle);
        }
    }

    [PunRPC]
    private void RemoteProjectileAttack(float aimAngle, Vector3 aimDirection, string projectileKey)
    {
        projectile = _allProjectiles[projectileKey];
        BaseProjectile projectileBase = projectile.GetComponent<BaseProjectile>();

        projectileBase.SetAimDirection(aimDirection);
        projectileBase.SetAimAngel(aimAngle);
        projectileBase.SetSenderCollider(gameObject);
        projectileBase.AddDamage(gameObject.GetComponent<BaseEntity>().GetBaseDamage());
        
        GameObject projectileInst = Instantiate(projectile, firePoint.position, firePoint.rotation);
    }
}