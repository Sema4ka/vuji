using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballSkill : BaseSkill
{
    [SerializeField] private GameObject projectile;

    private void Start() {
        this.energyCost = 20;
        this.castTime = 10f;
        this.cooldown = 1f;
    }

    public override IEnumerator UseSkill(GameObject caster)
    {
        base.UseSkill(caster);
        yield return new WaitForSeconds(10.0f);

        caster.GetComponent<PlayerProjectile>().Attack(projectile);
        
        yield return new WaitForSeconds(cooldown);
    }       
}
