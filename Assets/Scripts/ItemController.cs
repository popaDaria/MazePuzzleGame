using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PuzzleSystem
{
    public class ItemController : MonoBehaviour
    {
        [Header("Select Item Type")]
        [SerializeField] private bool redDoor;
        [SerializeField] private bool redKey;
        [SerializeField] private bool lever;
        [SerializeField] private bool button;
        [SerializeField] private bool hintPlaque;
        [SerializeField] private ItemInventory itemInventory;
        [SerializeField] private AudioSource audioSource;

        private ItemDoorController doorObject;
        private ButtonManager buttonManager;
        
        [Header("Lever anim")]
        [SerializeField] private string leverAnimName = "LeverAnim";
        private Animator leverAnim;

        [Header("Hint UI")]
        [SerializeField] private GameObject hintUI;
        [SerializeField] private int timeToShowUI = 1;

        private void Start()
        {

            if (redDoor)
            {
                doorObject = GetComponent<ItemDoorController>();
            }else if (lever)
            {
                leverAnim = GetComponentInChildren<Animator>();
            }else if (button)
            {
                buttonManager = GetComponentInParent(typeof(ButtonManager)) as ButtonManager;
            }
        }

        public void ObjectInteraction()
        {
            if (redDoor)
            {
                doorObject.PlayAnimation();
            }
            else
            {
                audioSource.Play();
                if (redKey)
                {
                    itemInventory.hasRedKey = true;
                    gameObject.SetActive(false);
                }
                else if (lever)
                {
                    itemInventory.leverOn = true;
                    leverAnim.Play(leverAnimName, 0, 0.0f);
                    gameObject.tag = "Untagged";
                }
                else if (button)
                {
                    buttonManager.PlayAnimation(gameObject);
                }
                else if (hintPlaque)
                {
                    StartCoroutine(SetHintShowingValue());
                }
            }
        }

        private IEnumerator SetHintShowingValue()
        {
            hintUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            hintUI.SetActive(false);
        }
    }
}

