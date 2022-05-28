using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class AcidFloor : MonoBehaviour
{
    [SerializeField] private GameObject poisonEffect;
    
    public PhotonView _myView;


    void OnCollisionEnter2D(Collision2D other)
    {
        if(CanDamageThisEnemy(other.gameObject));
            other.gameObject.GetComponent<BaseEntity>().AddEffect(poisonEffect);
    }


    private bool CanDamageThisEnemy(GameObject enemyGameObject)
    {
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
}
