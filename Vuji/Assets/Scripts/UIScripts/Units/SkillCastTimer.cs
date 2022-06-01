using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль для управления таймером каста скила целевой сущностью
/// </summary>
public class SkillCastTimer : MonoBehaviour
{
    [SerializeField, Tooltip("Текстовое поле для отображения оставшегося времени")] Text timerText; // Целевой текст таймера
    [SerializeField, Tooltip("Слайдер для отображения оставшегося времени")] Slider timerSlider; // Целевой слайдер таймера
    private float timerInterval; // Время каста
    private float timerValue; // Прошедшее время каста

    private void Start()
    {
        BaseSkill.onCast += StartTimer;
    }

    private void OnDestroy()
    {
        BaseSkill.onCast -= StartTimer;
    }
    /// <summary>
    /// Функция для события каста скила (Запустить таймер каста с указанным временем)
    /// </summary>
    /// <param name="time">Время каста умения</param>
    public void StartTimer(float time)
    {
        timerValue = 0f;
        timerInterval = time;
        timerSlider.minValue = 0f;
        timerSlider.maxValue = time;
    }

    // Update is called once per frame
    void Update()
    {
        timerValue += Time.deltaTime;
        float current = timerInterval - timerValue;
        if (current < 0f)
        {
            current = 0f;
        }
        timerSlider.value = current;
        timerText.text = current > 0f ? Convert.ToInt32(current).ToString() + "s" : "";

    }
}
