using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private List<BaseSkill> skills = new List<BaseSkill>();
    [SerializeField] private Inventory inventorie = new Inventory();

    #region Public Methods

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
        healthPoints -= healthDamage;
        if (healthPoints <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<PlayerScript>().KillPlayer();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        Debug.Log(entityName + " hp is " + healthPoints);
    }

    #endregion
}