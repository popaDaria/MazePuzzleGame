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
        [SerializeField] private bool normalDoor;
        [SerializeField] private bool keyOnlyDoor;
        [SerializeField] private bool key;
        [SerializeField] private bool lever;
        [SerializeField] private bool button;
        [SerializeField] private bool hintPlaque;
        [SerializeField] private ItemInventory itemInventory;
        
        [Header("For items only")]
        [SerializeField] private AudioSource audioSource;

        private ItemDoorController doorObject;
        private ItemButtonController buttonController;
        
        [Header("Lever anim")]
        [SerializeField] private string leverAnimName = "LeverAnim";
        private Animator leverAnim;

        [Header("Hint UI")]
        [SerializeField] private GameObject hintUI;
        [SerializeField] private int timeToShowUI = 1;

        private void Start()
        {

            if (normalDoor || keyOnlyDoor)
            {
                doorObject = GetComponent<ItemDoorController>();
            }else if (lever)
            {
                leverAnim = GetComponentInChildren<Animator>();
            }else if (button)
            {
                buttonController = GetComponentInParent(typeof(ItemButtonController)) as ItemButtonController;
            }
        }

        public void ObjectInteraction()
        {
            if (normalDoor)
            {
                doorObject.PlayAnimation();
            }else if (keyOnlyDoor)
            {
                doorObject.PlaySecondaryAnimation();
            }
            else
            {
                audioSource.Play();
                if (key)
                {
                    itemInventory.hasKey = true;
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
                    buttonController.PlayAnimation(gameObject);
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

