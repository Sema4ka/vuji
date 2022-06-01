using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Модуль для уравления звуком целевого источника
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField, Tooltip("Категория звука целевого источника")] string Category; // Категория звука целевого источника
    [SerializeField, Tooltip("Целевой источник звука")] AudioSource source; // Целевой источник звука
    private bool volumeSet = false; // Индикатор установки громкости звука целевого источника
    private float volume; // Громкость звука целевого источника
    // Start is called before the first frame update
    void Start()
    {
        if (SoundSettings.instance == null) return;
        volume = SoundSettings.instance.GetVolume(Category);
        source.volume = volume * SoundSettings.instance.GetVolume("General volume");
        SoundSettings.volumeChange += VolumeUpdated;
        volumeSet = true;
    }
    private void OnDestroy()
    {
        SoundSettings.volumeChange -= VolumeUpdated;
    }

    private void Update()
    {
        if (volumeSet || SoundSettings.instance == null) return;
        volume = SoundSettings.instance.GetVolume(Category);
        source.volume = volume * SoundSettings.instance.GetVolume("General volume");
        SoundSettings.volumeChange += VolumeUpdated;
        volumeSet = true;
    }
    /// <summary>
    /// Функция для события обновления настроек звука
    /// </summary>
    /// <param name="name">Название категории, громкость которой была обновлена</param>
    /// <param name="value">Новая громкость для категории</param>
    void VolumeUpdated(string name, float value)
    {
        if (name == Category)
        {
            source.volume = value * SoundSettings.instance.GetVolume("General volume");
            volume = value;
        }
        else if (name == "General volume")
        {
            source.volume = volume * value;
        }
    }
}
