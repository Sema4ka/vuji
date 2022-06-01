using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль для управления отдельным хп баром в панели списка участников команды
/// </summary>
public class TeamHPBarUI : MonoBehaviour
{
    [SerializeField, Tooltip("Слайдер для отображения хп целевой сущности")] Slider HealthBar; // Целевой слайдер
    [SerializeField, Tooltip("Текстовое поле для отображения имени целевого пользователя")] Text HealthBarText; // Целевое текстовое поле для имени пользователя
    [SerializeField, Tooltip("Текстовое поле для индикации смерти целевого игрока")] Text DeadText; // Целевое поле для индикации смерти сущности игрока
    private BaseEntity entity; // Целевая сущность
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
        if (entity.isDead)
        {
            HealthBar.maxValue = 0f;
            HealthBar.value = 0f;
            DeadText.text = "DEAD";
            return;
        }
        HealthBar.maxValue = entity.GetMaxHealthPoints();
        HealthBar.value = entity.GetHealthPoints();
    }
    /// <summary>
    /// Функция установки целевой сущности
    /// </summary>
    /// <param name="player">Целевая сущность</param>
    /// <param name="name">Имя целевого игрока</param>
    public void SetEntity(BaseEntity player, string name)
    {
        entity = player;
        HealthBarText.text = name;
    }
}