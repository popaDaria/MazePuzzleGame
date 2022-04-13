using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    [Header("Win UI")]
    [SerializeField] private int timeToClose = 15;
    [SerializeField] private GameObject winUI;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winUI.SetActive(true);
            Time.timeScale = 0;
            //TODO: see if necessary
            AudioListener.pause = true;
            PauseManager.paused = true;
            StartCoroutine(CloseGame());
        }
    }

    private IEnumerator CloseGame()
    {
        yield return new WaitForSecondsRealtime(timeToClose);
        Application.Quit();
    }
}
