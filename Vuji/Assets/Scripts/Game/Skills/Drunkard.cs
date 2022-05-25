using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunkard : BaseSkill
{
    [SerializeField] private GameObject drunkEffect;

    [Tooltip("Пират опустошает бутылку рома, на время опьянения пират получает на n меньше урона, но + n% к шансу промахнуться")]
    [SerializeField] private int additionalDefense = 5;

    [Tooltip("Время опьянения (секунды)")]
    [SerializeField] private float drunkTime = 10;

    public override IEnumerator UseSkill(GameObject caster, string key)
    {
        base.UseSkill(caster, key);
        if (caster.GetComponent<BaseEntity>().spendEnergy(energyCost) == false)
        {
            Debug.Log("Not enough energy to cast " + this.gameObject.name);
            yield break;
        }
        
        onCast?.Invoke(castTime); 
        caster.GetComponent<BaseEntity>().setIsCooldown(key, true);
        if(cancelMovementOnCast)
            caster.GetComponent<MovementPlayer>().cancelMovement(castTime);
        yield return new WaitForSeconds(castTime);

        // Сам скилл
        drunkEffect.GetComponent<Drunk>().additionalDefense = additionalDefense;
        drunkEffect.GetComponent<Drunk>().duration = drunkTime;
        onRelease?.Invoke(cooldown);

        caster.GetComponent<BaseEntity>().AddEffect(drunkEffect);
        // Сам скилл
        
        yield return new WaitForSeconds(cooldown);
        caster.GetComponent<BaseEntity>().setIsCooldown(key, false);
    }
}
