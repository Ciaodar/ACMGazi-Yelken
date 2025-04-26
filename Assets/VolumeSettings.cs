using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
   [SerializeField]
   private AudioMixer mixer;

   [SerializeField]
   private Slider musicSlider;

    [SerializeField]
    private Slider SfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {

            SetMusicVolume();
            SetSfxVolume();
        }

    }

    public void SetSfxVolume()
    {
        float volume = SfxSlider.value;
        mixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        SetMusicVolume();
        SetSfxVolume();
    }
}
