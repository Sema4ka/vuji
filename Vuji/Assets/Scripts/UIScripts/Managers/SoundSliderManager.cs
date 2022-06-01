using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль управления слайдером для настройки звука
/// </summary>
public class SoundSliderManager : MonoBehaviour
{
    [SerializeField, Tooltip("Текстовое поле названия категории звука")] Text text;
    [SerializeField, Tooltip("Слайдер громкости звука целевой категории")] Slider slider;

    public static Action<string, float> onValueChange;

    private float oldValue;
    /// <summary>
    /// Функция для привязки к изменению значения слайдера
    /// </summary>
    public void ValueChanged()
    {
        if (oldValue != slider.value)
        {
            onValueChange?.Invoke(text.text, slider.value);
            oldValue = slider.value;
        }

    }

    /// <summary>
    /// Установить название настройки звука
    /// </summary>
    /// <param name="newName">Новое название</param>
    public void SetName(string newName)
    {
        text.text = newName;
    }
    /// <summary>
    /// Установить значение настройки звука
    /// </summary>
    /// <param name="value">Новое значение</param>
    public void SetValue(float value)
    {
        oldValue = value;
        slider.value = value;
    }
}
