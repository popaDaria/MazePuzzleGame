using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public void ExitGame()
    {
        PlayerPrefs.SetFloat("time", TimeManager.currentTime);
        PlayerPrefs.Save();
        Application.Quit();
    }
}
