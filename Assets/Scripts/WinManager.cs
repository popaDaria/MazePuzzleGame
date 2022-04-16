using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [Header("Win UI")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject glitch1;
    [SerializeField] private GameObject glitch2;
    [SerializeField] private GameObject glitch3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0;
            PauseManager.paused = true;
            PauseManager.winPaused = true;
            winUI.SetActive(true);
            StartCoroutine(PlayGlitchAnimation());
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

    private IEnumerator PlayGlitchAnimation()
    {
        yield return new WaitForSecondsRealtime(Random.Range(1.0f, 2.0f));
        StartCoroutine(ShowGlitch(glitch1));
        yield return new WaitForSecondsRealtime(Random.Range(5.0f, 10.0f));
        StartCoroutine(ShowGlitch(glitch2));
        yield return new WaitForSecondsRealtime(Random.Range(7.0f, 15.0f));
        StartCoroutine(ShowGlitch(glitch3));
    }

    private static IEnumerator ShowGlitch(GameObject glitchObject)
    {
        glitchObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.15f);
        glitchObject.SetActive(false);
    }
}
