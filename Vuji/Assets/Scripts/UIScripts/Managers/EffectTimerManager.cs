using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс для управления таймером эффекта
/// </summary>
public class EffectTimerManager : MonoBehaviour
{
    [SerializeField, Tooltip("Изображение для спрайта целевого эффекта")] Image targetedSprite; // Изображения для отображения спрайта эффекта
    [SerializeField, Tooltip("Текстовое поле для отображения оставшегося времени эффекта")] Text targetedText; // Текст таймера эффекта
    [SerializeField, Tooltip("Модуль для установки текста подсказки при наведении")] TooltipTextUI tooltipText; // Текст подсказки при наведении


    private BaseEffect targetedEffect; // отслеживаемый эффект

    private float timerInterval = 0f; // Продолжительность эффекта
    private float timerValue = 0f; // Прошедшее время действия эффекта
    // Start is called before the first frame update
    void Start()
    {
        targetedText.text = "";
    }


    /// <summary>
    /// Установка эффекта на данных
    /// </summary>
    /// <param name="effect">Целевой эффект</param>
    public void SetEffect(BaseEffect effect)
    {
        targetedSprite.sprite = effect.effectSprite;
        timerInterval = effect.duration;
        targetedEffect = effect;
        tooltipText.text = "\"" + effect.effectName + "\"\n" + effect.description + "\n" + "Duration: " + effect.duration + "s";
    }

    /// <summary>
    /// Отсчет времени действия эффекта
    /// </summary>
    void Update()
    {
        if (targetedEffect == null) return;
        timerValue += Time.deltaTime;
        float current = timerInterval - timerValue;
        if (current < 0f)
        {
            current = 0f;
            Destroy(gameObject);
        }
        targetedText.text = current > 0f ? Convert.ToInt32(current).ToString() + "s" : "";
    }
}
