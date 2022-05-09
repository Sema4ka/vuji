using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlurryOfFire : BaseSkill
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootTime;
    [SerializeField] private float timeBetweenShoot;

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

        for(int i = 0; i < shootTime / timeBetweenShoot; i++)
        {
            caster.GetComponent<PlayerProjectile>().Attack(projectile);
            yield return new WaitForSeconds(timeBetweenShoot);
        }
        yield return new WaitForSeconds(cooldown);
        caster.GetComponent<BaseEntity>().setIsCooldown(key, false);
    }       
}
