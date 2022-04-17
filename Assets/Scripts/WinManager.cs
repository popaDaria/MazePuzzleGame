using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WinManager : MonoBehaviour
{
    [Header("Win Zone UIs")] 
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject glitch1;
    [SerializeField] private GameObject glitch2;
    [SerializeField] private GameObject glitch3;
    [SerializeField] private AudioSource music;
    
    [Header("Monologue UI")]
    [SerializeField] private GameObject monologueUI;
    private TextMeshProUGUI monologueText;
    public static bool monologuePause;

    [Header("Thanks UI")]
    [SerializeField] private GameObject thanksPanelUI;

    private void Start()
    {
        monologueText = monologueUI.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowMonologue());
        }
    }

    public void TryAgain()
    {
        PauseManager.winPaused = false;
        PauseManager.paused = false;
        Time.timeScale = 1;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        StartCoroutine(ShowThanks());
    }

    private IEnumerator PlayGlitchAnimation()
    {
        yield return new WaitForSecondsRealtime(Random.Range(1.0f, 2.0f));
        StartCoroutine(ShowGlitch(glitch1));
        yield return new WaitForSecondsRealtime(Random.Range(5.0f, 10.0f));
        StartCoroutine(ShowGlitch(glitch2));
        yield return new WaitForSecondsRealtime(Random.Range(7.0f, 15.0f));
        StartCoroutine(ShowGlitch(glitch3));
    }

    private IEnumerator ShowGlitch(GameObject glitchObject)
    {
        glitchObject.SetActive(true);
        music.Pause();
        yield return new WaitForSecondsRealtime(0.15f);
        glitchObject.SetActive(false);
        music.UnPause();
    }
    
    private IEnumerator ShowThanks()
    {
        winUI.SetActive(false);
        thanksPanelUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        thanksPanelUI.SetActive(false);
        Application.Quit();
    }

    private IEnumerator ShowMonologue()
    {
        monologuePause = true;
        monologueUI.SetActive(true);
        controlsUI.SetActive(false);
        PauseManager.paused = true;
        PauseManager.winPaused = true;
        yield return new WaitForSecondsRealtime(1.0f);
        monologueText.text = "Finally! I made it out.";
        yield return new WaitForSecondsRealtime(2.0f);
        monologueText.text = "Wait";
        yield return new WaitForSecondsRealtime(1.0f);
        monologueText.text = "This looks awfully familiar...";
        yield return new WaitForSecondsRealtime(4.0f);
        monologueText.text = "What-";
        yield return new WaitForSecondsRealtime(0.5f);
        monologuePause = false;
        monologueText.text = "";
        monologueUI.SetActive(false);
        Time.timeScale = 0;
        winUI.SetActive(true);
        StartCoroutine(PlayGlitchAnimation());
    }
}