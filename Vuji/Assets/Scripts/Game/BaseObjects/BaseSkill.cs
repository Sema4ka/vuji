using System;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class BaseSkill : MonoBehaviour 
{
    [Tooltip("Название способности")]
    [SerializeField] protected string skillName;

    [Tooltip("Описание способности")]
    [SerializeField] protected string skillDescription;

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
        yield return new WaitForSeconds(0.0f);
    }
}
