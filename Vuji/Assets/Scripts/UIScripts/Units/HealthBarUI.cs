using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль управления баром хп в нижней части экрана
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [SerializeField, Tooltip("Целевой слайдер для отображения количеста хп")] Slider HealthBar; // Целевой слайдер
    [SerializeField, Tooltip("Целевое текстовое поле для отображения количеста хп")] Text HealthBarText; // Целевое текстовое поле
    private BaseEntity entity; // Целевая сущность
    // Start is called before the first frame update
    void Start()
    {
        HealthBar.minValue = 0f;
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
    }
    void OnSpawn(GameObject player)
    {
        entity = player.GetComponent<BaseEntity>();
    }
    // Update is called once per frame
    void Update()
    {
        if (entity == null)
        {
            HealthBar.maxValue = 0f;
            HealthBar.value = 0f;
            HealthBarText.text = "";
            return;
        }
        if (entity.isDead)
        {
            HealthBar.maxValue = 0f;
            HealthBar.value = 0f;
            HealthBarText.text = "";
            return;
        }
        HealthBar.maxValue = entity.GetMaxHealthPoints();
        HealthBar.value = entity.GetHealthPoints();
        HealthBarText.text = entity.GetHealthPoints().ToString() + "/" + entity.GetMaxHealthPoints().ToString();
    }
    /// <summary>
    /// Установить целевую сущность
    /// </summary>
    /// <param name="player">Новая целевая сущность</param>
    public void SetEntity(BaseEntity player)
    {
        entity = player;
    }
}
