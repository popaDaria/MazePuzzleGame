using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PuzzleSystem
{
    public class ItemButtonController : MonoBehaviour
    {

        [Header("Button solution")]
        [SerializeField] private int[] solution = {1, 0, 2};
        [SerializeField] private ItemInventory itemInventory;
        private int[] inputtedSolution = {-1, -1, -1};
        
        [Header("Button anim")]
        private Animator buttonAnim;
        [SerializeField] private string buttonAnimName = "ButtonAnim";
        private AudioSource correctSolutionAudio;
        private AudioSource wrongSolutionAudio;
        
        [Header("Button UI")]
        [SerializeField] private int timeToShowUI = 1;
        [SerializeField] private GameObject wrongSolutionUI;


        private void Start()
        {
            var sources = GetComponents<AudioSource>();
            if (sources[0].clip.name.Contains("wrong"))
            {
                wrongSolutionAudio = sources[0];
                correctSolutionAudio = sources[1];
            } else {
                correctSolutionAudio = sources[0];
                wrongSolutionAudio = sources[1];
            }
        }

        public void PlayAnimation(GameObject buttonGameObject)
        {
            buttonAnim = buttonGameObject.GetComponent<Animator>();
            buttonAnim.Play(buttonAnimName, 0, 0.0f);

            var digit = buttonGameObject.name switch
            {
                "Button 0" => 0,
                "Button 1" => 1,
                "Button 2" => 2,
                _ => -1
            };

            inputtedSolution[0] = inputtedSolution[1];
            inputtedSolution[1] = inputtedSolution[2];
            inputtedSolution[2] = digit;

            if (!inputtedSolution.Contains(-1))
            {
                if (solution[0] == inputtedSolution[0] && solution[1] == inputtedSolution[1] && solution[2] == inputtedSolution[2])
                {
                    correctSolutionAudio.Play();
                    var colliders= gameObject.GetComponentsInChildren<MeshCollider>();
                    foreach (var collider in colliders)
                    {
                        Destroy(collider);
                    }
                    itemInventory.buttonsPressed = true;
                }
                else
                {
                    StartCoroutine(ShowWrongSolution());
                    inputtedSolution = new[] {-1, -1, -1};
                }
            }

        }

        private IEnumerator ShowWrongSolution()
        {
            yield return new WaitForSeconds(timeToShowUI / 3.0f);
            wrongSolutionAudio.Play();
            wrongSolutionUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            wrongSolutionUI.SetActive(false);
        }

    }
}

