using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    private Vector2 horizontalInput;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform feetTransform;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    [SerializeField] private MouseCameraController mouseCameraController;
    private Vector2 mouseInput;
    
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -30f;
    private Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] private float jumpHeight = 3.5f;
    private bool jump;
    [SerializeField] private AudioSource footstepSound;

    private enum PlayerStance
    {
        Stand,
        Crouch,
        Prone
    }
    private PlayerStance playerStance;
    
    [Header("Stance")]
    [SerializeField] private float stanceSmoothingFactor;
    [SerializeField] private CharacterStance playerStandStance;
    [SerializeField] private CharacterStance playerCrouchStance;
    [SerializeField] private CharacterStance playerProneStance;
    
    private float cameraHeight;
    private float cameraVelocity;
    private Vector3 capsuleCenterVelocity;
    private float capsuleHeightVelocity;
    private static readonly float STANCE_ERROR_MARGIN = 0.05f;

    private void Awake()
    {
        playerInputSystem = new PlayerInputSystem();
        cameraHeight = mouseCameraController.mainCamera.localPosition.y;
        
        var positionX = PlayerPrefs.GetFloat("positionX", 257);
        var positionZ = PlayerPrefs.GetFloat("positionZ", 10);
        gameObject.transform.position = new Vector3(positionX, 0.0f, positionZ);
    }
    
    private void OnEnable()
    {
        playerInputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputSystem.Player.Disable();
    }
    
    private void Update()
    {
        mouseCameraController.ReceiveInput(mouseInput);
        CalculateStance();
        
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        if (horizontalInput != Vector2.zero && footstepSound.isPlaying == false && isGrounded && !PauseManager.paused)
        {
            footstepSound.volume = Random.Range(0.15f, 0.35f);
            footstepSound.pitch = Random.Range(0.9f, 1.3f);
            footstepSound.Play();
        }

        Vector3 movement = transform.right * horizontalInput.x + transform.forward * horizontalInput.y;
        characterController.Move(movement*speed*Time.deltaTime);

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                if (!PauseManager.paused)
                {
                    StartCoroutine(PlayLandingSound());
                }
            }
            jump = false;
        }
        
        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private IEnumerator PlayLandingSound()
    {
        yield return new WaitForSeconds(0.5f);
        footstepSound.Play();
    }

    void OnMove(InputValue movement)
    {
        horizontalInput = movement.Get<Vector2>();
    }
    
    void OnMouseX(InputValue movement)
    {
        mouseInput.x = movement.Get<float>();
    }
    
    void OnMouseY(InputValue movement)
    {
        mouseInput.y = movement.Get<float>();
    }

    void OnJump()
    {
        jump = true;
    }

    void OnCrouch()
    {
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        
        if (StanceCheck(playerCrouchStance.stanceCollider.height))
        {
            return;
        }
        playerStance = PlayerStance.Crouch;
    }
    
    void OnProne()
    {
        if (playerStance == PlayerStance.Prone)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        playerStance = PlayerStance.Prone;
    }

    private void CalculateStance()
    {
        Vector3 localPosition = mouseCameraController.mainCamera.localPosition;
        var currentStance = playerStandStance;

        if (playerStance == PlayerStance.Crouch)
        {
            currentStance = playerCrouchStance;
        }else if (playerStance == PlayerStance.Prone)
        {
            currentStance = playerProneStance;
        }
        
        cameraHeight = Mathf.SmoothDamp(localPosition.y, currentStance.cameraHeight,ref cameraVelocity,stanceSmoothingFactor);
        mouseCameraController.mainCamera.localPosition = new Vector3( localPosition.x,cameraHeight, localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.stanceCollider.height, ref capsuleHeightVelocity, stanceSmoothingFactor);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.stanceCollider.center, ref capsuleCenterVelocity, stanceSmoothingFactor);
    }
    
    private bool StanceCheck(float checkHeight)
    {
        var position = feetTransform.position;
        Vector3 start = new Vector3(position.x, position.y + STANCE_ERROR_MARGIN + characterController.radius, position.z);
        Vector3 end = new Vector3(position.x, position.y - STANCE_ERROR_MARGIN - characterController.radius + checkHeight, position.z);
        
        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }
}

[Serializable]
public class CharacterStance
{
    public float cameraHeight;
    public CapsuleCollider stanceCollider;
}
