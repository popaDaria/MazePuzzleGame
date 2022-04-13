using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private void Start()
    {
        var fullscreen = PlayerPrefs.GetString("fullscreenMode","True");
        Screen.fullScreenMode = fullscreen.Equals("True") ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (PauseManager.paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
