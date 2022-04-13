using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSystem
{
    public class ItemDoorController : MonoBehaviour
    {

        private Animator[] doorAnim;
        private bool doorOpen = false;

        [Header("Animation Names")] 
        [SerializeField] private string[] openAnimationNames = {"GateLeftAnim", "GateRightAnim"};
        // [SerializeField] private string closeAnimationName = "DoorClose";

        [Header("Door UI")]
        [SerializeField] private int timeToShowUI = 1;
        [SerializeField] private GameObject showDoorLockedUI;
        [SerializeField] private GameObject showDoorOpenedUI;
        
        [Header("Controls")]
        [SerializeField] private ItemInventory itemInventory;
        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;
        [SerializeField] private AudioSource doorLockedSound;
        [SerializeField] private AudioSource doorUnlockSound;
        [SerializeField] private AudioSource doorOpeningSound;

        private void Awake()
        {
            doorAnim = gameObject.GetComponentsInChildren<Animator>();
        }

        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }
        
        private IEnumerator ShowDoorLocked()
        {
            showDoorLockedUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            showDoorLockedUI.SetActive(false);
        }
        
        private IEnumerator ShowDoorOpened()
        {
            showDoorOpenedUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            showDoorOpenedUI.SetActive(false);
        }
        
        public void PlayAnimation()
        {
            if (itemInventory.hasRedKey || itemInventory.leverOn || itemInventory.buttonsPressed)
            {
                if (!doorOpen && !pauseInteraction)
                {
                    doorUnlockSound.Play();
                    doorOpeningSound.Play();
                    StartCoroutine(ShowDoorOpened());
                    for (int i = 0; i < openAnimationNames.Length; i++)
                    {
                        doorAnim[i].Play(openAnimationNames[i],0,0.0f);
                    }
                    doorOpen = true;
                    itemInventory.hasRedKey = false;
                    itemInventory.leverOn = false;
                    itemInventory.buttonsPressed = false;
                    Destroy(gameObject.GetComponent<BoxCollider>());
                    StartCoroutine(PauseDoorInteraction());
                }
                /*else if (doorOpen && !pauseInteraction)
                {
                    doorAnim.Play(closeAnimationName,0,0.0f);
                    doorOpen = false;
                    StartCoroutine(PauseDoorInteraction());
                }*/
            }
            else
            {
                doorLockedSound.Play();
                StartCoroutine(ShowDoorLocked());
            }
        }
    }
}

