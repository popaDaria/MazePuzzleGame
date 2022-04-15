using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    [Header("Win UI")]
    [SerializeField] private GameObject winUI;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winUI.SetActive(true);
            Time.timeScale = 0;
            PauseManager.paused = true;
        }
    }

    public void TryAgain()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
