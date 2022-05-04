using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public static Action<bool> timerEnd;
    [SerializeField] Text timerText;
    [SerializeField] Slider timerSlider;
    [SerializeField] float timerInterval;
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
