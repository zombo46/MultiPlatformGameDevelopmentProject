using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Public variables for player settings
    public Camera playerCamera;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchMultiplier = 0.5f;

    // Interaction settings (Input System only)
    public Transform interactionPoint;
    public float interactionRange = 1.5f;

    // Private/internal state
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f; // pitch
    private float rotationY = 0f; // yaw
    private CharacterController characterController;
    private bool isCrouched = false;
    private bool canMove = true;

    // Keep originals to avoid repeatedly multiplying/dividing speeds
    private float baseWalkSpeed;
    private float baseRunSpeed;

    // Vertical velocity tracked separately for consistent grounding/jumping
    private float verticalVelocity = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("PlayerMovement requires a CharacterController.");
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        baseWalkSpeed = walkSpeed;
        baseRunSpeed = runSpeed;

        rotationY = transform.eulerAngles.y;
        if (playerCamera != null)
        {
            float camPitch = playerCamera.transform.localEulerAngles.x;
            if (camPitch > 180f) camPitch -= 360f;
            rotationX = camPitch;
        }

        if (interactionPoint == null)
        {
            interactionPoint = this.transform;
        }

        // Ensure controller center corresponds to default height
        characterController.height = Mathf.Max(0.1f, defaultHeight);
        characterController.center = new Vector3(0f, characterController.height / 2f, 0f);
    }

    void Update()
    {
        Movement();
        Interaction();
    }

    private void Movement()
    {
        if (!canMove)
            return;

        // Input System:
        float inputZ = 0f; // forward/back
        float inputX = 0f; // left/right

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputZ += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputZ -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputX += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputX -= 1f;
        }

        // Running (Keyboard shift)
        bool isRunning = (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed);

        // Determine horizontal speed (applies crouch multiplier if active)
        float speed = (isRunning ? baseRunSpeed : baseWalkSpeed) * (isCrouched ? crouchMultiplier : 1f);

        // Horizontal movement (relative to player)
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 horizontalMove = (forward * inputZ + right * inputX);
        if (horizontalMove.sqrMagnitude > 1f) horizontalMove = horizontalMove.normalized;
        horizontalMove *= speed;

        // Grounding & vertical velocity
        bool jumpPressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);
        
        if (characterController.isGrounded)
        {
            // small downward force to keep grounded
            if (verticalVelocity < 0f) verticalVelocity = -1f;

            if (jumpPressed && !isCrouched)
            {
                verticalVelocity = jumpPower;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Assemble final move vector and move controller
        moveDirection = horizontalMove;
        moveDirection.y = verticalVelocity;
        characterController.Move(moveDirection * Time.deltaTime);

        // Crouch toggle
        bool crouchToggled = (Keyboard.current != null && Keyboard.current.leftCtrlKey.wasPressedThisFrame);
        if (crouchToggled)
            ToggleCrouch();

        // Mouse look (Input System)
        Vector2 mouseDelta = Vector2.zero;
        if (Mouse.current != null)
            mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;

        rotationX += -mouseY * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        rotationY += mouseX * lookSpeed;

        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    private void ToggleCrouch()
    {
        if (!isCrouched)
        {
            // Enter crouch
            characterController.height = Mathf.Max(0.1f, crouchHeight);
            characterController.center = new Vector3(0f, characterController.height / 2f, 0f);
            isCrouched = true;
        }
        else
        {
            // Exit crouch - naive uncrouch (no ceiling check). If you need safety, add a Physics check before standing.
            characterController.height = Mathf.Max(0.1f, defaultHeight);
            characterController.center = new Vector3(0f, characterController.height / 2f, 0f);
            isCrouched = false;
        }
    }

    // Public to allow other scripts to enable / disable player movement.
    public void SetCanMove(bool enabled)
    {
        canMove = enabled;
        if (!canMove)
        {
            // stop any residual movement and rotation immediately
            moveDirection = Vector3.zero;
            verticalVelocity = 0f;
        }
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    private void Interaction()
    {
        bool interactPressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame);

        if (!interactPressed) return;

        Collider[] colliders = Physics.OverlapSphere(interactionPoint.position, interactionRange);
        foreach (Collider collider in colliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(collider);
                break;
            }
        }
    }
}