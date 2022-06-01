using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль управления баром энергии в нижней части экрана
/// </summary>
public class EnergyBarUI : MonoBehaviour
{
    [SerializeField, Tooltip("Целевой слайдер для отображения количеста энергии")] Slider EnergyBar;
    [SerializeField, Tooltip("Целевое текстовое поле для отображения количеста энергии")] Text EnergyBarText;
    private BaseEntity entity; // Целевая сущность
    // Start is called before the first frame update
    void Start()
    {
        EnergyBar.minValue = 0f;
        SpawnPlayers.OnSpawn += OnSpawn;
    }
    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
    }
    /// <summary>
    /// Функция для события появления игрока
    /// </summary>
    /// <param name="player">Объект игрока</param>
    void OnSpawn(GameObject player)
    {
        entity = player.GetComponent<BaseEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (entity == null)
        {
            EnergyBar.maxValue = 0f;
            EnergyBar.value = 0f;
            EnergyBarText.text = "";
            return;
        }
        if (entity.isDead)
        {
            EnergyBar.maxValue = 0f;
            EnergyBar.value = 0f;
            EnergyBarText.text = "";
            return;
        }
        EnergyBar.maxValue = entity.GetMaxEnergyPoints();
        EnergyBar.value = entity.GetEnergyPoints();
        EnergyBarText.text = entity.GetEnergyPoints().ToString() + "/" + entity.GetMaxEnergyPoints().ToString();
    }
    public void SetEntity(BaseEntity player)
    {
        entity = player;
    }
}
