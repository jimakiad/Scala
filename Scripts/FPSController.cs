using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip walkingSound;
    private bool isWalkingSoundPlaying = false;
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    // Head bobbing variables
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    float defaultPosY = 0;
    public bool isCrouching = false;
    public float crouchSpeed = 3f;
    private float originalHeight;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;
    public Animator playerAnimator;

    // Variables for head bobbing
    private float timer = 0.0f;
    private float bobbingSpeedMultiplier = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store the default vertical position of the camera
        defaultPosY = playerCamera.transform.localPosition.y;
        originalHeight = characterController.height;

        audioSource = GetComponent<AudioSource>();

        // Set the audio clip for walking
        audioSource.clip = walkingSound;
    }

    // Update is called once per frame
    async void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion


        if ((curSpeedX != 0 || curSpeedY != 0) && !isRunning && characterController.isGrounded)
        {
            playerAnimator.SetBool("Walking", true);
            if (!isWalkingSoundPlaying)
            {
                audioSource.Play();
                isWalkingSoundPlaying = true;
            }

        }
        else
        {
            
            playerAnimator.SetBool("Walking", false);
            if (isWalkingSoundPlaying)
            {
                audioSource.Stop();
                isWalkingSoundPlaying = false;
            }
        }

        
        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            playerAnimator.SetBool("Jumps", true);

        }
        else
        {
            moveDirection.y = movementDirectionY;
            
        }

        if (!characterController.isGrounded && movementDirectionY > 0)
        {
            playerAnimator.SetBool("Jumps", false);
        }


        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion
        
        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Head Bobbing

        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
          
            waveslice = 0.0f;
            timer = 0.0f;
        }
        else
        {
            
            waveslice = Mathf.Sin(timer);
            timer += bobbingSpeed * bobbingSpeedMultiplier;

            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);

           
            float newY = defaultPosY + translateChange;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, newY, playerCamera.transform.localPosition.z);
        }
        else
        {
           
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultPosY, playerCamera.transform.localPosition.z);
        }

        #endregion

        

        if (transform.localPosition.y < -2)
        {
      
            transform.localPosition = new Vector3(5.06f, 1.16f, 0);
        }

        #region Handles Crouching

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }

        #endregion
    }
    void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            // Crouch
            characterController.height = originalHeight / 2f;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed; 
        }
        else
        {
            // Stand up
            characterController.height = originalHeight;
            walkSpeed = 6f;
            runSpeed = 12f; 
        }
    }
    




}

//https://www.youtube.com/watch?v=qQLvcS9FxnY fps controller used