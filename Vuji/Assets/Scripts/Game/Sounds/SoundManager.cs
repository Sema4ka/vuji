using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] string Category;
    [SerializeField] AudioSource source;
    private bool volumeSet = false;
    private float volume;
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
