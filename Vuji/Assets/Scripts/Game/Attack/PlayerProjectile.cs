using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private Transform firePoint;

    private Vector3 _mousePosition;
    private Vector3 _aimDirection;
    private float _aimAngle;
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
            _aimDirection = _mousePosition - rotatePoint.position;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
            rotatePoint.rotation = Quaternion.Euler(0, 0, _aimAngle);
            if (Input.GetMouseButtonDown(0))
            {
                RemoteProjectileAttack(_aimAngle, _aimDirection);
                _view.RPC("RemoteProjectileAttack", RpcTarget.All, _aimAngle, _aimDirection);
            }
        }
    }

    [PunRPC]
    private void RemoteProjectileAttack(float aimAngle, Vector3 aimDirection)
    {
        rotatePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
        projectile.GetComponent<BaseProjectile>().SetAimDirection(aimDirection);
        projectile.GetComponent<BaseProjectile>().SetAimAngel(aimAngle);

        GameObject projectileInst = Instantiate(projectile, firePoint.position, firePoint.rotation);
    }
}