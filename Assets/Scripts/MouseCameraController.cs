using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraController : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] public Transform mainCamera;
    [SerializeField] private float xClamp = 85f;
    [SerializeField] private float sensitivityX = 8f;
    [SerializeField] private float sensitivityY = 0.5f;
    
    private float mouseX, mouseY;
    private float xRotation = 0f;

    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }

    private void Update()
    {
        if (!PauseManager.paused)
        {
            transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = xRotation;
            mainCamera.eulerAngles = targetRotation;
        }
    }
}
