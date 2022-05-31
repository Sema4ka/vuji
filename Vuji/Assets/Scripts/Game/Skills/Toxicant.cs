using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Toxicant : BaseSkill
{
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
        caster.GetComponent<PlayerAOE>().Attack("AcidFloor");
        // Сам скилл
        onRelease?.Invoke(cooldown);
        yield return new WaitForSeconds(cooldown);
        caster.GetComponent<BaseEntity>().setIsCooldown(key, false);
    }   
}
