using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballSkill : BaseSkill
{
    [SerializeField] private GameObject projectile;

    public override IEnumerator UseSkill(GameObject caster, string key)
    {
        base.UseSkill(caster, key);
        caster.GetComponent<BaseEntity>().setIsCooldown(key, true);
        yield return new WaitForSeconds(castTime);

        caster.GetComponent<PlayerProjectile>().Attack(projectile);

        yield return new WaitForSeconds(cooldown);
        caster.GetComponent<BaseEntity>().setIsCooldown(key, false);
    }       
}
