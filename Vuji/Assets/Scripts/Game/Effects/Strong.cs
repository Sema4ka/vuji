using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strong : BaseEffect
{
    [Tooltip("Усиление")]
    [SerializeField] public int additionalDamage = 30;

    public override void ApplyEffect(GameObject entity)
    {
        base.ApplyEffect(entity);
        entity.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(StrongEffect(entity));
    }

    public IEnumerator StrongEffect(GameObject entity){
        entity.GetComponent<BaseEntity>().IncreaseDamage(additionalDamage);
        yield return new WaitForSeconds(duration);
        entity.GetComponent<BaseEntity>().DecreaseDamage(additionalDamage);
    }
}
