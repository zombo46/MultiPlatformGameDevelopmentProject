using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void Update()
    {
        if (!canMove)
            return;

        // Input
        float inputZ = Input.GetAxis("Vertical");   // forward/back
        float inputX = Input.GetAxis("Horizontal"); // left/right
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Determine horizontal speed (applies crouch multiplier if active)
        float speed = (isRunning ? baseRunSpeed : baseWalkSpeed) * (isCrouched ? crouchMultiplier : 1f);

        // Horizontal movement (world-space)
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 horizontalMove = (forward * inputZ + right * inputX) * speed;

        // Grounding & vertical velocity
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }

        // Mouse look
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

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
            characterController.height = crouchHeight;
            isCrouched = true;
        }
        else
        {
            characterController.height = defaultHeight;
            isCrouched = false;
        }
    }
}