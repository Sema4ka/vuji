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
    [SerializeField] private GameObject healthBarPrefab;
    private GameObject healthBar;
    private float width;
    private float height;
    #endregion
    private void Start()
    {

        maxHealthPoints = healthPoints > maxHealthPoints ? healthPoints : maxHealthPoints;

        width = GetComponent<RectTransform>().sizeDelta.x;
        height = GetComponent<RectTransform>().sizeDelta.y;

        float healthPercentage = healthPoints / maxHealthPoints;

        Vector3 pos = transform.position;
        healthBar = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity);
        healthBar.transform.SetParent(transform, false);

        healthBar.GetComponent<RectTransform>().position = pos + new Vector3(0, height * 0.6f, 0);
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height * 0.1f);
        healthBar.GetComponent<Slider>().value = healthPercentage;
    }

    private void Update()
    {
        float healthPercentage = healthPoints / maxHealthPoints;
        healthBar.GetComponent<Slider>().value = healthPercentage;
        // healthPercent.sizeDelta = new Vector2(healthPercentage, height * 0.1f);
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