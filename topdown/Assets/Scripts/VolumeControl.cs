﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public string volumeName;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeName, 0.75f);
    }

    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("Master Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Master Volume", sliderValue);
    }
    public void SetSFXLevel(float sliderValue)
    {
        mixer.SetFloat("SFX Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFX Volume", sliderValue);
    }
    public void SetVolume(float value)
    {
        mixer.SetFloat("exposedParamName", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("exposedParamName", value);
    }

}
