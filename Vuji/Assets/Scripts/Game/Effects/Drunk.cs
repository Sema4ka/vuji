using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunk : BaseEffect
{
    [Tooltip("Пират опустошает бутылку рома, на время опьянения пират получает на n меньше урона, но + n% к шансу промахнуться")]
    [SerializeField] public int additionalDefense;

    public override void ApplyEffect(GameObject entity)
    {
        base.ApplyEffect(entity);
        entity.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(DrunkEffect(entity));
    }

    public IEnumerator DrunkEffect(GameObject entity){
        entity.GetComponent<BaseEntity>().IncreaseDefense(additionalDefense);
        yield return new WaitForSeconds(duration);
        entity.GetComponent<BaseEntity>().DecreaseDefense(additionalDefense);
    }
}
