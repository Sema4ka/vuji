using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : BaseEffect
{
    [Tooltip("Усокрение")]
    [SerializeField] public int additionalSpeed = 2;

    public override void ApplyEffect(GameObject entity)
    {
        base.ApplyEffect(entity);
        entity.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(SpeedEffect(entity));
    }

    public IEnumerator SpeedEffect(GameObject entity){
        entity.GetComponent<BaseEntity>().IncreaseSpeed(additionalSpeed);
        yield return new WaitForSeconds(duration);
        entity.GetComponent<BaseEntity>().DecreaseSpeed(additionalSpeed);
    }
}
