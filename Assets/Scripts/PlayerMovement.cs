using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))] // Ensures a CharacterController component is attached to the GameObject.
public class PlayerMovement : MonoBehaviour
{
    // Public variables for player settings
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchMultiplier = 0.5f;
    // Private variables for internal state - do not modify these directly
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f; // pitch
    private float rotationY = 0f; // yaw (explicit)
    private CharacterController characterController;
    private bool isCrouched = false;
    // Flag to control if the player can move - set to false to disable movement.
    private bool canMove = true;

    void Start()
    {
        // Get the CharacterController component attached to this GameObject and lock the cursor to the center of the screen. Used for first-person camera control.
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize rotation state from existing transforms so pitch continues from the current camera angle.
        rotationY = transform.eulerAngles.y;
        if (playerCamera != null)
        {
            float camPitch = playerCamera.transform.localEulerAngles.x;
            if (camPitch > 180f) camPitch -= 360f; // convert 0..360 to -180..180
            rotationX = camPitch;
        }
    }

    void Update()
    {
        // Calculate the forward and right directions based on the player's current rotation.
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Determine if the player is running based on the Left Shift key and calculate movement speeds. If the player is not allowed to move, speeds are set to zero - useful for later implementing features like cutscenes or menus.
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        // Calculate the new movement direction based on input and apply gravity and jumping mechanics.
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        // isGrounded flag prevents jumping while in mid-air.
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        // Only applies gravity if player is in the air.
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        // Crouch mechanic toggled by the Left Control key, adjusting player height and movement speed accordingly.
        if (Input.GetKeyDown(KeyCode.LeftControl) && canMove)
        {
            if (!isCrouched)
            {
                characterController.height = crouchHeight;
                isCrouched = true;
                walkSpeed *= crouchMultiplier;
                runSpeed *= crouchMultiplier;
            }
            else
            {
                isCrouched = false;
                characterController.height = defaultHeight;
                walkSpeed /= crouchMultiplier;
                runSpeed /= crouchMultiplier;
            }
        }
        // Move the character controller based on the calculated movement direction.
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle player rotation based on mouse movement, clamping vertical look angle to prevent excessive rotation (e.g., looking directly up or down).
        if (canMove)
        {
            // Use explicit yaw/pitch state and raw axes for nicer input.
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            rotationX += -mouseY * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            rotationY += mouseX * lookSpeed;

            // Apply pitch to the camera (local rotation) and yaw to the player transform.
            if (playerCamera != null)
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}