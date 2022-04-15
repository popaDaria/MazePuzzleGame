using System;
using System.Collections;
using System.Collections.Generic;
using PuzzleSystem;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    [SerializeField] private GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = door.GetComponent<ItemDoorController>();
            controller.PlayCloseDoorAnimation();
        }
    }
}
