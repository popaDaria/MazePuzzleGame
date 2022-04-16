using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private bool timerActive;
    public static float currentTime;
    [SerializeField] private TextMeshProUGUI timeText;

    private void Start()
    {
        currentTime = PlayerPrefs.GetFloat("time", 0.0f);
        timerActive = true;
    }

    private void Update()
    {
        timerActive = !PauseManager.paused;
        if (timerActive)
        {
            currentTime += Time.deltaTime;
        }
        var time = TimeSpan.FromSeconds(currentTime);
        timeText.text = time.ToString(@"hh\:mm\:ss");
    }
}
