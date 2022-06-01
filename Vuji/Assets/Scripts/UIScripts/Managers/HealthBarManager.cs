using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль для управления полоской жизней над сущностью
/// </summary>
public class HealthBarManager : MonoBehaviour
{
    [Tooltip("Целевой слайдер для отображения хп")] public Slider slider; // Целевой слайдер для отображения хп
    [SerializeField, Tooltip("Цвет для низких значений хп")] Color Low; // Цвет для низких значений хп
    [SerializeField, Tooltip("Цвет для высоких значений хп")] Color High; // Цвет для высоких значений хп
    private Vector3 offset; // Отклюнение от позиции сущности

    /// <summary>
    /// Изменение особенностей объектов, необходимое для корректного отображения бара хп
    /// </summary>
    private void Start()
    {
        GetComponentInParent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        slider.fillRect.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        slider.fillRect.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        slider.fillRect.parent.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        slider.fillRect.parent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
    }

    /// <summary>
    /// Установить текущие и максимальные хп сущности
    /// </summary>
    /// <param name="health">Текущее хп</param>
    /// <param name="maxHp">Максимальное хп</param>
    public void SetHealth(float health, float maxHp)
    {
        gameObject.SetActive(health < maxHp);
        slider.value = health;
        slider.maxValue = maxHp;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, slider.normalizedValue);
    }

    /// <summary>
    /// Обновить позицию бара над сущностью
    /// </summary>
    void Update()
    {
        Vector3 temp = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        slider.transform.position = new Vector3(temp.x, temp.y, 0);
    }
    /// <summary>
    /// Установить отклонение от позиции сущности
    /// </summary>
    /// <param name="newOffset">Новое отклонение</param>
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Текущее отклонение от позиции сущности</returns>
    public Vector3 GetOffset()
    {
        return offset;
    }
}
