using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PuzzleSystem
{
    public class ItemRaycast : MonoBehaviour
    {
        [SerializeField] private int rayLength = 5;
        [SerializeField] private LayerMask layerMaskInteract;
        [SerializeField] private string excludeLayerName;

        private ItemController raycastedObject;

        [SerializeField] private Image crosshair;
        private bool isCrosshairActive;
        private bool doOnce;

        private string interactibleTag = "InteractiveObject";
        [SerializeField] private GameObject showItemInteractUI;


        private void Update()
        {
            var keyboard = Keyboard.current;
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
            
            if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
            {
                if (hit.collider.CompareTag(interactibleTag))
                {
                    if (!doOnce)
                    {
                        raycastedObject = hit.collider.gameObject.GetComponent<ItemController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;
                    showItemInteractUI.SetActive(true);

                    if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
                    {
                        raycastedObject.ObjectInteraction();
                    }
                }
            }
            else
            {
                showItemInteractUI.SetActive(false);
                if (isCrosshairActive)
                {
                    CrosshairChange(false);
                    doOnce = false;
                }
            }
        }

        private void CrosshairChange(bool on)
        {
            if (on && !doOnce)
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
                isCrosshairActive = false;
            }
        }
    }
}

