using UnityEngine;
using UnityEngine.InputSystem;

public class
    PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool m_isPlayer1;
    [SerializeField]
    private Transform m_opponent;

    private float gravity = -9.82f;
    private float groundedGravity = -0.05f;

    private Animator animator;
    private PlayerInput playerInput;
    private CharacterController characterController;

    // move
    private bool isForward = false;
    private Vector3 currMove;
    private float moveX;
    private float moveSpeed = 5;
    private bool isTurned = true;
    private bool isForwardAnimation = false;
    private float zPos = 0.5f;
    // jump
    private float jumpHeight = 0.3f;
    private bool isJumpingPressed = false;
    private bool isJumping = false;
    private bool isJumpAnimation = false;
    private float initJumpVelocity;
    private float fallFaster = 2.0f;
    // crouch
    private bool isCrouchPressed = false;
    private bool isCrouching = false;
    // block
    private bool isBlocking = false;
    private bool isBlockAnimation = false;
    // attack
    private bool isAttack1 = false;
    private bool isAttack2 = false;
    private bool isAttack3 = false;

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
        Attack();
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

    public void OnAttack1(InputAction.CallbackContext context)
    {
        isAttack1 = context.ReadValueAsButton();
    }
    public void OnAttack2(InputAction.CallbackContext context)
    {
        isAttack2 = context.ReadValueAsButton();
    }
    public void OnAttack3(InputAction.CallbackContext context)
    {
        isAttack3 = context.ReadValueAsButton();
    }

    public void OnAttackEnd(InputAction.CallbackContext context)
    {
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        animator.ResetTrigger("attack3");
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
            // fixing z index if needed
            if (transform.position.z != 0.5f)
            {
                currMove.z = (zPos - transform.position.z) * 0.05f;
            }
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

    private void Attack()
    {
        if (isAttack1 || (isAttack1 && isJumpingPressed)) // idle
        {
            animator.SetTrigger("attack1");
        }
        if ((isAttack2 && isJumpingPressed && isForward) || (isAttack2 && isForward)) // directional
        {
            animator.SetTrigger("attack2");
        }
        if ((isAttack3 && isForward && isCrouching) || (isAttack3 && isCrouching))  // crouch
        {
            animator.SetTrigger("attack3");
        }
    }

    private void ApplyGravity()
    {
        bool isFalling = currMove.y <= 0.0f || !isJumpingPressed;
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
            playerInput.P1_Controls.Jump.performed += OnJump;
            playerInput.P1_Controls.Jump.canceled += OnJump;
            playerInput.P1_Controls.Crouch.started += OnCrouch;
            playerInput.P1_Controls.Crouch.performed += OnCrouch;
            playerInput.P1_Controls.Crouch.canceled += OnCrouch;
            playerInput.P1_Controls.Attack1.started += OnAttack1;
            playerInput.P1_Controls.Attack1.performed += OnAttack1;
            playerInput.P1_Controls.Attack1.canceled += OnAttackEnd;
            playerInput.P1_Controls.Attack2.started += OnAttack2;
            playerInput.P1_Controls.Attack2.performed += OnAttack2;
            playerInput.P1_Controls.Attack2.canceled += OnAttackEnd;
            playerInput.P1_Controls.Attack3.started += OnAttack3;
            playerInput.P1_Controls.Attack3.performed += OnAttack3;
            playerInput.P1_Controls.Attack3.canceled += OnAttackEnd;
        }
        else
        {
            playerInput.P2_Controls.Move.started += OnMove;
            playerInput.P2_Controls.Move.performed += OnMove;
            playerInput.P2_Controls.Move.canceled += OnMove;
            playerInput.P2_Controls.Jump.performed += OnJump;
            playerInput.P2_Controls.Jump.canceled += OnJump;
            playerInput.P2_Controls.Crouch.started += OnCrouch;
            playerInput.P2_Controls.Crouch.performed += OnCrouch;
            playerInput.P2_Controls.Crouch.canceled += OnCrouch;
            playerInput.P2_Controls.Attack1.started += OnAttack1;
            playerInput.P2_Controls.Attack1.performed += OnAttack1;
            playerInput.P2_Controls.Attack1.canceled += OnAttackEnd;
            playerInput.P2_Controls.Attack2.started += OnAttack2;
            playerInput.P2_Controls.Attack2.performed += OnAttack2;
            playerInput.P2_Controls.Attack2.canceled += OnAttackEnd;
            playerInput.P2_Controls.Attack3.started += OnAttack3;
            playerInput.P2_Controls.Attack3.performed += OnAttack3;
            playerInput.P2_Controls.Attack3.canceled += OnAttackEnd;
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
