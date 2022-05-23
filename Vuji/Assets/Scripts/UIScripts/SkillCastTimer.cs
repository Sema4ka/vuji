using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCastTimer : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] Slider timerSlider;
    private float timerInterval;
    private float timerValue;

    private void Start()
    {
        BaseSkill.onCast += StartTimer;
    }

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
        timerText.text = current > 0f?Convert.ToInt32(current).ToString() + "s":"";

    }
}
