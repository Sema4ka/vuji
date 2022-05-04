using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseSkill : MonoBehaviour 
{
    [Tooltip("Количество маны требуемое для использования способности")]
    [SerializeField] protected int energyCost;

    [Tooltip("Время, которое проходит между нажатием кнопки и активацией способности")]
    [SerializeField] protected float castTime;

    [Tooltip("Задержка перед повторным использованием способности")]
    [SerializeField] protected float cooldown;

    public virtual IEnumerator UseSkill(GameObject caster, string key) {
        caster.GetComponent<BaseEntity>().setIsCooldown(key, true);
        yield return new WaitForSeconds(castTime);
    }
}
