using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : BaseEffect
{
    [SerializeField] private int fireDamage = 5;
    [SerializeField] private float damageTickSeconds = 1.0f;
    [SerializeField] private int repeatCount = 5;

    public override void ApplyEffect(GameObject entity)
    {
        base.ApplyEffect(entity);
        StartCoroutine(OnFireEffect(entity));
        
    }

    public IEnumerator OnFireEffect(GameObject entity){
        for(int i = 0; i < repeatCount; i++)
        {
            entity.GetComponent<BaseEntity>().TakeDamage(fireDamage);
            yield return new WaitForSeconds(damageTickSeconds);
        }
    }
}
