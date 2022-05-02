using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseSkill : MonoBehaviour 
{
    // "Количество маны требуемое для использования способности"
    protected int energyCost { get; set; }

    // "Время, которое проходит между нажатием кнопки и активацией способности"
    protected float castTime { get; set; }

    // "Задержка перед повторным использованием способности"
    protected float cooldown { get; set; }

    public virtual IEnumerator UseSkill(GameObject caster) {
        yield return new WaitForSeconds(castTime);
    }
}
