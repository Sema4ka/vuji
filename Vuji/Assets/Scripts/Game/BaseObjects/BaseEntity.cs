using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private List<BaseSkill> skills = new List<BaseSkill>();
    

    public void AddEffect(BaseEffect effect)
    {
        effect.ApplyEffect(this);
    }

    public void UseSkill(BaseSkill skill)
    {
        skill.UseSkill(this);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetHealthPoints()
    {
        return healthPoints;
    }

    public string GetEntityName()
    {
        return entityName;
    }
    
    public void TakeDamage(int healthDamage)
    {
        this.healthPoints -= healthDamage;
        Debug.Log(this.entityName + " hp is " + this.healthPoints);
    }
}