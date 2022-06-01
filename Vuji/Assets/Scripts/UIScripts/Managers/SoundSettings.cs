using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль для управления настройками звука
/// </summary>
public class SoundSettings : MonoBehaviour
{
    [SerializeField] DataBase dataBase;
    [SerializeField, Tooltip("Панель настроек звука")] RectTransform soundSettingsContent;
    [SerializeField, Tooltip("Префаб настройки звука")] GameObject soundSlider;
    public static Action<string, float> volumeChange;
    public static SoundSettings instance;

    // Sound volumes list
    Dictionary<string, float> volumeList = new Dictionary<string, float>();

    private void OnDestroy()
    {
        SoundSliderManager.onValueChange -= SoundSliderValueChange;
    }

    // Load all sounds
    void Start()
    {
        instance = this;
        SoundSliderManager.onValueChange += SoundSliderValueChange;
        int nowY = -75;
        var settings = dataBase.GetSettings();
        foreach (Setting setting in settings)
        {
            if (!setting.name.EndsWith("volume")) continue;
            GameObject slider = Instantiate(soundSlider);
            slider.transform.SetParent(soundSettingsContent, false);
            slider.transform.localPosition = new Vector2(335, nowY);
            var manager = slider.GetComponent<SoundSliderManager>();
            manager.SetName(setting.name);
            manager.SetValue(float.Parse(setting.value));
            volumeList[setting.name] = float.Parse(setting.value);
            nowY -= 50;
        }
    }
    /// <summary>
    /// Функция для события установки значения звука пользователем
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SoundSliderValueChange(string name, float value)
    {
        volumeList[name] = value;
        dataBase.SetSetting(name, value.ToString());
        volumeChange?.Invoke(name, value);
    }
    /// <summary>
    /// Получить значение звука для заданной категории
    /// </summary>
    /// <param name="name">Название категории</param>
    /// <returns>Громкость категории</returns>
    public float GetVolume(string name)
    {
        return volumeList[name];
    }

}
