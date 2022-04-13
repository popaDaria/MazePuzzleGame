using System;
using System.Collections;
using System.Collections.Generic;
using PuzzleSystem;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private PauseAction action;
    public static bool paused;

    [Header("References")]
    [SerializeField] private GameObject menuUI;
    [SerializeField] private Transform playerPosition;
    
    private SettingsManager settingsManager;
    
    [Header("Save UI")]
    [SerializeField] private float timeToShowUI = 1.0f;
    [SerializeField] private GameObject saveUI;

    private void Awake()
    {
        action = new PauseAction();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Start()
    {
        action.Pause.PauseGame.performed += _ => DeterminePause();
        settingsManager = gameObject.GetComponent<SettingsManager>();
    }

    private void DeterminePause()
    {
        if (!SettingsManager.inSettings)
        {
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        else
        {
            settingsManager.CloseSettings();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        menuUI.SetActive(true);
        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuUI.SetActive(false);
        paused = false;
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("positionX", playerPosition.transform.position.x);
        PlayerPrefs.SetFloat("positionZ", playerPosition.transform.position.z);
        PlayerPrefs.Save();
        StartCoroutine(ShowSaveUI());
    }
    
    private IEnumerator ShowSaveUI()
    {
        saveUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeToShowUI);
        saveUI.SetActive(false);
    }

}
