using UnityEngine;
using UnityEngine.InputSystem;

public class
    PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool m_isPlayer1;
    [SerializeField]
    private Transform m_opponent;

    private Vector3 screenBounds;
    private float gravity = -9.82f;
    private float groundedGravity = -0.05f;

    private Animator animator;
    private PlayerInput playerInput;
    private CharacterController characterController;

    // move
    private bool isForward = false;
    private Vector2 currMove;
    private float moveX;
    private float moveSpeed = 5;
    private bool isTurned = true;
    private bool isForwardAnimation = false;
    // private float verticalVelicoty;

    // jump
    private float jumpHeight = 0.25f;
    private bool isJumpingPressed = false;
    private bool isJumping = false;
    private bool isJumpAnimation = false;
    private float initJumpVelocity;
    // crouch
    private bool isCrouchPressed = false;
    private bool isCrouching = false;
    // block
    private bool isBlocking = false;
    private bool isBlockAnimation = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = new PlayerInput();
        SubscribeActions();
        float timeToUp = jumpHeight / 2;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToUp, 2);
        initJumpVelocity = (2 * jumpHeight) / timeToUp;

        if (m_isPlayer1)
        {
            isTurned = false;
        }

    }


    // Update is called once per frame
    private void Update()
    {
        Turn();
        Jump();
        Crouch();
        Move();
        Forward();
        Block();
        ApplyGravity();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveX = context.ReadValue<float>();
        currMove.x = moveX;
        isForward = isTurned ? moveX < 0 : moveX > 0;
        isBlocking = isTurned ? moveX > 0 : moveX < 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumpingPressed = context.ReadValueAsButton();    
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouchPressed = context.ReadValueAsButton();
    }

    public void Jump()
    {
        if (!isJumping && isJumpingPressed && characterController.isGrounded)
        {
            currMove.y = initJumpVelocity + 0.5f;
            animator.SetBool("isJumping", true);
            isJumping = true;
            isJumpAnimation = true;
        }
        else if(isJumping && !isJumpingPressed && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    public void Crouch()
    {
        if (!isCrouching && isCrouchPressed && characterController.isGrounded)
        {
            animator.SetBool("isCrouching", true);
            isCrouching = true;
        }
        else if (isCrouching && !isCrouchPressed && characterController.isGrounded)
        {
            animator.SetBool("isCrouching", false);
            isCrouching = false;
        }
    }

    public void Block()
    {
        if (isBlocking && !isBlockAnimation)
        {
            animator.SetBool("isBlocking", true);
            isBlockAnimation = true;
            // to do: transition to block attack animation if being hit (trigger: blockAttack)
        }
        else if (!isBlocking && isBlockAnimation)
        {
            animator.SetBool("isBlocking", false);
            isBlockAnimation = false;
        }
    }

    public void Move()
    {
        if (isCrouching)
        {
            characterController.Move(Vector3.zero);
        }
        else
        {
            characterController.Move(currMove * moveSpeed * Time.deltaTime);
        }
    }

    private void Turn()
    {
        if (!isTurned && transform.position.x > m_opponent.position.x)
        {
            m_opponent.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
            isTurned = !isTurned;
        }
        else if (isTurned && transform.position.x < m_opponent.position.x)
        {
            m_opponent.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
            isTurned = !isTurned;
        }

    }
    private void Forward()
    {
        if (isForward && !isForwardAnimation)
        {
            animator.SetBool("isForward", true);
            isForwardAnimation = true;
        }
        else if (!isForward && isForwardAnimation)
        {
            animator.SetBool("isForward", false);
            isForwardAnimation = false;
        }
    }

    private void ApplyGravity()
    {
        bool isFalling = currMove.y <= 0.0f || !isJumpingPressed;
        float fallFaster = 2.0f;
        if (characterController.isGrounded)
        {
            if (isJumpAnimation)
            {
                animator.SetBool("isJumping", false);
                isJumpAnimation = false;
            }
            currMove.y = groundedGravity;
        }
        else if (isFalling)
        {
            float prevVelocity = currMove.y;
            float newVelocity = currMove.y + (gravity * fallFaster * Time.deltaTime);
            float nextVelocity = (prevVelocity + newVelocity) * 0.5f;
            currMove.y = nextVelocity;
        }
        else
        {
            float prevVelocity = currMove.y;
            float newVelocity = currMove.y + (gravity * Time.deltaTime);
            float nextVelocity = (prevVelocity + newVelocity) * 0.5f;
            currMove.y = nextVelocity;
        }
    }

    private void SubscribeActions()
    {
        if (m_isPlayer1)
        {
            playerInput.P1_Controls.Move.started += OnMove;
            playerInput.P1_Controls.Move.performed += OnMove;
            playerInput.P1_Controls.Move.canceled += OnMove;
            playerInput.P1_Controls.Jump.started += OnJump;
            playerInput.P1_Controls.Jump.canceled += OnJump;
            playerInput.P1_Controls.Crouch.started += OnCrouch;
            playerInput.P1_Controls.Crouch.performed += OnCrouch;
            playerInput.P1_Controls.Crouch.canceled += OnCrouch;
        }
        else
        {
            playerInput.P2_Controls.Move.started += OnMove;
            playerInput.P2_Controls.Move.performed += OnMove;
            playerInput.P2_Controls.Move.canceled += OnMove;
            playerInput.P2_Controls.Jump.started += OnJump;
            playerInput.P2_Controls.Jump.canceled += OnJump;
            playerInput.P2_Controls.Crouch.started += OnCrouch;
            playerInput.P2_Controls.Crouch.performed += OnCrouch;
            playerInput.P2_Controls.Crouch.canceled += OnCrouch;
        }
    }

    private void OnEnable()
    {
        if (m_isPlayer1)
        {
            playerInput.P1_Controls.Enable();
        }
        else
        {
            playerInput.P2_Controls.Enable();
        }

    }

    private void OnDisable()
    {
        if (m_isPlayer1)
        {
            playerInput.P1_Controls.Disable();
        }
        else
        {
            playerInput.P2_Controls.Disable();
        }
    }
}
