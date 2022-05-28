using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Poison : BaseEffect
{
    [SerializeField] public int posionDamage = 5;
    [SerializeField] public float damageTickSeconds = 1.0f;

    public override void ApplyEffect(GameObject entity)
    {
        base.ApplyEffect(entity);
        entity.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(PoisonEffect(entity));
    }

    public IEnumerator PoisonEffect(GameObject entity){
        for(int i = 0; i < duration/damageTickSeconds; i++)
        {
            Debug.Log("Poisoned " + entity.name);
            entity.GetComponent<PhotonView>().RPC("TakeDamageRemote", RpcTarget.All, entity.GetComponent<PhotonView>().ViewID, posionDamage);
            yield return new WaitForSeconds(damageTickSeconds);
        }
    }
}
