using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAOE : MonoBehaviour
{

    [SerializeField] Transform firePoint;

    [Serializable]
    public struct AOEstruct
    {
        public string AOEKey;
        public GameObject AOE;
    }
    [SerializeField] private AOEstruct[] AOEs;
    private Dictionary<string, GameObject> _allAOEs = new Dictionary<string, GameObject>();

    private PhotonView _view;

    void Start()
    {
        _view = gameObject.GetComponent<PhotonView>();

        // Заполнение нормального словаря всех проджектайлов
        if (AOEs.Length != 0)
            for (int i = 0; i < AOEs.Length; i++)
            {
                Debug.Log(AOEs[i].AOEKey + " " + AOEs[i].AOE);
                this._allAOEs[AOEs[i].AOEKey] = AOEs[i].AOE;
            }
    }

    public void Attack(string AOEKey)
    {
        _view.RPC("CreateAOE", RpcTarget.All, AOEKey);
    }

    [PunRPC]
    private void CreateAOE(string AOEKey)
    {
        GameObject AOE = _allAOEs[AOEKey];
        BaseAOE aoeBase = AOE.GetComponent<BaseAOE>();
        aoeBase.SetSenderCollider(this.gameObject); 

        Instantiate(AOE, firePoint.position, Quaternion.identity);
    }
}
