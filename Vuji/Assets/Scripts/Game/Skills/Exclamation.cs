using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exclamation : BaseSkill
{
    [SerializeField] GameObject speedEffect;
    [SerializeField] GameObject strongEffect;

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
        caster.GetComponent<BaseEntity>().AddEffect(speedEffect);
        caster.GetComponent<BaseEntity>().AddEffect(strongEffect);
        // Сам скилл
        onRelease?.Invoke(cooldown);
        yield return new WaitForSeconds(cooldown);
        caster.GetComponent<BaseEntity>().setIsCooldown(key, false);
    }   
}
