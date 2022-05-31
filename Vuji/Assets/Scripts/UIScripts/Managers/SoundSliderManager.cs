using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderManager : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Slider slider;

    public static Action<string, float> onValueChange;

    private float oldValue;

    public void ValueChanged()
    {
        if (oldValue != slider.value)
        {
            onValueChange?.Invoke(text.text, slider.value);
            oldValue = slider.value;
        }

    }


    public void SetName(string newName)
    {
        text.text = newName;
    }
    public void SetValue(float value)
    {
        oldValue = value;
        slider.value = value;
    }
}
