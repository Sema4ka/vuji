using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : BaseEffect
{
    public OnFire()
    {
        effectName = "OnFire";
        description = "ты гориш";

        duration = 10f;
    }

    public override void ApplyEffect(BaseEntity entity)
    {
        base.ApplyEffect(entity);
        StartCoroutine(OnFireEffect(entity));
        
    }

    public IEnumerator OnFireEffect(BaseEntity entity){
        entity.TakeDamage(10);
        yield return new WaitForSeconds(1f);
        entity.TakeDamage(10);
    }
}
