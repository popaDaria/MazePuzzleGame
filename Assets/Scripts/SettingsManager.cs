using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private AudioMixer mainMixer;

    [Header("UI elements")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider envSlider;

    [Header("Min Volumes")] 
    [SerializeField] private float minMusicVolume = -30.0f;
    [SerializeField] private float minEnvVolume = -10.0f;
    [SerializeField] private float minFXVolume = -30.0f;
    
    public static bool inSettings;
    private readonly float muteVolume = -80.0f;
    
    private void Start()
    {
        var fxVolume = PlayerPrefs.GetFloat("fxVolume", -15.0f);
        var musicVolume = PlayerPrefs.GetFloat("musicVolume", -15.0f);
        var envVolume = PlayerPrefs.GetFloat("environmentVolume", -4.5f);
        
        mainMixer.SetFloat("fxVolume", fxVolume);
        fxSlider.value = fxVolume;
        mainMixer.SetFloat("musicVolume", musicVolume);
        musicSlider.value = musicVolume;
        mainMixer.SetFloat("environmentVolume", envVolume);
        envSlider.value = envVolume;
        
        var fullscreen = PlayerPrefs.GetString("fullscreenMode","True");
        fullscreenToggle.isOn = fullscreen.Equals("True");
    }

    public void OpenSettings()
    {
        menuUI.SetActive(false);
        settingsUI.SetActive(true);
        inSettings = true;
    }

    public void CloseSettings()
    {
        PlayerPrefs.Save();
        settingsUI.SetActive(false);
        menuUI.SetActive(true);
        inSettings = false;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        PlayerPrefs.SetString("fullscreenMode", isFullscreen.ToString());
    }
    
    public void SetFXVolume(float volume)
    {
        if (Math.Abs(volume - (minFXVolume)) > 0.01f)
        {
            mainMixer.SetFloat("fxVolume", volume);
            PlayerPrefs.SetFloat("fxVolume", volume);
        }
        else
        {
            mainMixer.SetFloat("fxVolume", muteVolume);
            PlayerPrefs.SetFloat("fxVolume", muteVolume);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        if (Math.Abs(volume - (minMusicVolume)) > 0.01f)
        {
            mainMixer.SetFloat("musicVolume", volume);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }
        else
        {
            mainMixer.SetFloat("musicVolume", muteVolume);
            PlayerPrefs.SetFloat("musicVolume", muteVolume);
        }
    }
    
    public void SetEnvironmentVolume(float volume)
    {
        if (Math.Abs(volume - (minEnvVolume)) > 0.01f)
        {
            mainMixer.SetFloat("environmentVolume", volume);
            PlayerPrefs.SetFloat("environmentVolume", volume);
        }
        else
        {
            mainMixer.SetFloat("environmentVolume", muteVolume);
            PlayerPrefs.SetFloat("environmentVolume", muteVolume);
        }
    }

}
