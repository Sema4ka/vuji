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

    [Tooltip("Запрет на движение игрока во время использования способности")]
    [SerializeField] protected bool cancelMovementOnCast;

    public static Action<float> onCast;

    public virtual IEnumerator UseSkill(GameObject caster, string key) {
        caster.GetComponent<BaseEntity>().setIsCooldown(key, true);
        onCast?.Invoke(castTime); 
        yield return new WaitForSeconds(0.0f);
    }
}
