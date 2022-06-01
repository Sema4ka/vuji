using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль управления таймером выбора класса
/// </summary>
public class TimerManager : MonoBehaviour
{
    public static Action<bool> timerEnd;
    [SerializeField, Tooltip("Текст таймера выбора класса")] Text timerText;
    [SerializeField, Tooltip("Слайдер для отображения прогресса таймера")] Slider timerSlider;
    [SerializeField, Tooltip("Заданное время для выбора класса")] float timerInterval;
    private float timerValue;
    // Start is called before the first frame update
    void Start()
    {
        timerValue = 0f;
        timerSlider.minValue = 0f;
        timerSlider.maxValue = timerInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timerValue += Time.deltaTime;
        float current = timerInterval - timerValue;
        if (current < 0f)
        {
            timerEnd?.Invoke(true);
            current = 0f;
        }
        timerSlider.value = current;
        timerText.text = Convert.ToInt32(current).ToString();

    }
}
