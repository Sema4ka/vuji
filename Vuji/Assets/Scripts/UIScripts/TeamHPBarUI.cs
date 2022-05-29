using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamHPBarUI : MonoBehaviour
{
    [SerializeField] Slider HealthBar;
    [SerializeField] Text HealthBarText;
    [SerializeField] Text DeadText;
    private BaseEntity entity;
    // Start is called before the first frame update
    void Start()
    {
        HealthBar.minValue = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (entity == null)
        {
            HealthBar.maxValue = 0f;
            HealthBar.value = 0f;
            return;
        }
        if (entity.isDead) // Тот кто писал это - идиот
        {
            HealthBar.maxValue = 0f;
            HealthBar.value = 0f;
            DeadText.text = "DEAD";
            return;
        }
        HealthBar.maxValue = entity.GetMaxHealthPoints();
        HealthBar.value = entity.GetHealthPoints();
    }
    public void SetEntity(BaseEntity player, string name)
    {
        entity = player;
        HealthBarText.text = name;
    }
}