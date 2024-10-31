using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Transform cameraTransform;

    //Movement and Jump configuration parameters
    [Header("PlayerConfiguration")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float gravity = Physics.gravity.y;

    //Input fields for movements and look actions
    private Vector2 moveInput;
    private Vector2 lookInput;

    //Velocity and rotation variables
    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation;

    //Is Sprinting
    private bool isSprinting;

    //Camera look sensitivity and max angle to limitvertical rotation
    [SerializeField] private float lookSensitivity = 0.5f;
    private float maxLookAngle = 80f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;


        //Hide Mouse Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        // Manage Player Movement 
        MovePlayer();
        // Manage Camera Rotation
        LookAround();
    }

    /// <summary>
    /// Receives movement input from InputSystem
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }

    /// <summary>
    /// Receive look Input from the InputSystem
    /// </summary>
    /// <param name="context"></param>
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }


    /// <summary>
    /// Receive jump input from Input System and triggers jump if grounded
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context) 
    {
        //if Player is touching ground
        if(characterController.isGrounded)
        {
            //Calculate the require velocity for a jump
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

    }

    /// <summary>
    /// Receive Sprint input from Input System and change isSprinting state
    /// </summary>
    /// <param name="context"></param>
    public void Sprint(InputAction.CallbackContext context)
    {
        //When action started or maintained
        isSprinting = context.started || context.performed;
    }

    /// <summary>
    /// Handles Player Movement based on input and applies gravity
    /// </summary>
    private void MovePlayer()
    {
        //Falling down
        if (characterController.isGrounded)
        {
            //Restart vertical velocity when touch ground
            verticalVelocity = 0f;
        }
        else
        {   
            //When is falling down increment velocity with gravity and time
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);


        //Movement
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        float targetSpeed = isSprinting ? speed * multiplier : speed;
        characterController.Move(moveDirection * targetSpeed* Time.deltaTime);

        //Apply gravity constantly to posibility jump
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }


    /// <summary>
    /// Handles Camera rotation based on look inputs
    /// </summary>
    private void LookAround()
    {
        //Horizontal rotation (Y-axis) based on sesitivity and Input
        float horizontalRotation = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * horizontalRotation);

        //Verticcal rotation (X-axis) with Clamping to prevent over-rotation
        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

    }




}
