using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Модуль по управлению списком эффектов
/// </summary>
public class EffectsListManager : MonoBehaviour
{
    [SerializeField, Tooltip("Панель для привязки эффектов")] RectTransform effectsPanel; // Панель списка эффектов
    [SerializeField, Tooltip("Префаб таймера эффекта")] GameObject effectPrefab; // Префаб таймера эффекта
    private BaseEntity targetEntity; // Сущность, для которой необходимо отслеживать накладываемые эффекты

    /// <summary>
    /// Добавление функции к событию появления игрока
    /// </summary>
    void Start()
    {
        SpawnPlayers.OnSpawn += OnSpawn;
    }

    /// <summary>
    /// Отмена вызова функии при появлении игрока и, при наличии, при наложении эффекта на сущность
    /// </summary>
    private void OnDestroy()
    {
        SpawnPlayers.OnSpawn -= OnSpawn;
        if (targetEntity != null) targetEntity.OnEffectApply -= OnEffect;
    }
    /// <summary>
    /// Функция для события появления игрока
    /// </summary>
    /// <param name="player">Объект игрока</param>
    void OnSpawn(GameObject player)
    {
        targetEntity = player.GetComponent<BaseEntity>();
        targetEntity.OnEffectApply += OnEffect;
    }
    /// <summary>
    /// Функция для события наложения эффекта на сущность
    /// </summary>
    /// <param name="effect">Накладываемый эффект</param>
    /// <param name="entity">Целевая сущность</param>
    void OnEffect(BaseEffect effect, BaseEntity entity)
    {
        if (targetEntity != entity) return;
        GameObject effectTimer = Instantiate(effectPrefab);
        effectTimer.transform.SetParent(effectsPanel, false);
        effectTimer.GetComponent<EffectTimerManager>().SetEffect(effect);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
