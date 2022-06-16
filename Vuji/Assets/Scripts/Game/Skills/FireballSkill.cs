using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireballSkill : BaseSkill
{
    [SerializeField] private GameObject projectile;

    public override IEnumerator UseSkill(GameObject caster, string key)
    {
        base.UseSkill(caster, key);

        if (caster.GetComponent<PlayerEntity>().spendEnergy(energyCost) == false)
        {
            Debug.Log("Not enough energy to cast " + this.gameObject.name);
            yield break;
        }
        
        onCast?.Invoke(castTime); 
        caster.GetComponent<PlayerEntity>().setIsCooldown(key, true);
        if(cancelMovementOnCast)
            caster.GetComponent<MovementPlayer>().cancelMovement(castTime);
        yield return new WaitForSeconds(castTime);


        caster.GetComponent<PlayerProjectile>().Attack("Fireball");
        onRelease?.Invoke(cooldown);
        yield return new WaitForSeconds(cooldown);
        caster.GetComponent<PlayerEntity>().setIsCooldown(key, false);
    }       
}
