using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private string doorOpenAnim = "DoorOpen";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.GetComponent<Animator>().Play(doorOpenAnim, 0, 0.0f);
            Destroy(door,2);
            Destroy(gameObject);
        }
    }
}
