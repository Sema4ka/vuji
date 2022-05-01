using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "baseEntityName";
    [SerializeField] private float healthPoints = 100.0f;
    [SerializeField] private float maxHealthPoints = 100.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private List<BaseSkill> skills = new List<BaseSkill>();

    #region HealthBar
    [SerializeField] HealthBarManager healthBar;
    #endregion
    private void Start()
    {
        maxHealthPoints = Math.Max(maxHealthPoints, healthPoints);
        float height = 1.0f;
        healthBar.SetOffset(new Vector3(0, height * 0.6f, 0));
        healthBar.SetHealth(healthPoints, maxHealthPoints);
        gameObject.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        healthBar.slider.fillRect.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        healthBar.slider.fillRect.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        healthBar.slider.fillRect.parent.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        healthBar.slider.fillRect.parent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
    }

    private void Update()
    {
        healthBar.SetHealth(healthPoints, maxHealthPoints);
    }

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

    public float GetMaxHealthPoints()
    {
        return maxHealthPoints;
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