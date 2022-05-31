using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePassiveSkill : MonoBehaviour
{

    [Tooltip("Название способности")]
    [SerializeField] protected string skillName;

    [Tooltip("Описание способности")]
    [SerializeField] protected string skillDescription;

    public string GetName()
    {
        return skillName;
    }

    public string GetDescription()
    {
        return skillDescription;
    }

    public virtual void ActivatePassiveSkill(GameObject caster) {
        Debug.Log(caster.GetComponent<BaseEntity>().GetEntityName() + " activated passive skill");
    }
}
