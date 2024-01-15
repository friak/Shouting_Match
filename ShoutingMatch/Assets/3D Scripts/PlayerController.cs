using UnityEngine;

public class
    PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool m_isPlayer1;
    public bool IsPlayer1 { get { return m_isPlayer1; } private set { } }
    [SerializeField]
    private Transform m_opponent; // will be set by the fight scene manager later
    public Transform Opponent { get { return m_opponent; } private set { } }

    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    private CharacterController characterController;
    // attack
    [SerializeField]
    private Attack attack;
    [SerializeField]
    private AttackScriptableAsset standardAttack;
    private bool isLightPressed = false;
    [SerializeField]
    private AttackScriptableAsset forwardAttack;
    private bool isMediumPressed = false;
    [SerializeField]
    private AttackScriptableAsset crouchAttack;
    private bool isHeavyPressed = false;
    private AttackLevel attackLeve = AttackLevel.NONE;

    // move
    private bool isForward = false;
    private Vector3 currMove;
    private float moveX;
    private float moveSpeed = 5;
    private bool isTurned = true;
    private bool isForwardAnimation = false;
    private float zPos = 0.5f;
    private float gravity = -9.82f;
    private float groundedGravity = -0.05f;
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
    public bool IsBlocking { get { return isBlocking; } private set { } }
    private bool isBlockAnimation = false;

    private bool isGameOver = false;
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        int direction = m_isPlayer1 ? 1 : -1;
        transform.Rotate(0.0f, direction * 90.0f, 0.0f, Space.Self);
        isTurned = m_isPlayer1 ? false : true;
        float timeToUp = jumpHeight / 2;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToUp, 2);
        initJumpVelocity = (2 * jumpHeight) / timeToUp;
        isTurned = m_isPlayer1 ? false : true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (attack.IsAttacking)
        {
            // animator should freeze in current state
            return;
        }
        if (player.IsDead && !isGameOver) GameOver();
        Attack();
        Turn();
        Jump();
        Crouch();
        Move();
        Forward();
        Block();
        ApplyGravity();
    }

    public void SetupController(bool isPlayer1, Transform opponent)
    {
        m_isPlayer1 = isPlayer1;
        m_opponent = opponent;
        // reset facing
        int direction = isPlayer1 ? 1 : -1;
        transform.Rotate(0.0f, direction * 90.0f, 0.0f, Space.Self);
        isTurned = m_isPlayer1 ? false : true;
    }

    public void SetMoveFromArduino(float move)
    {
        moveX = move;
        currMove.x = moveX;
        isForward = isTurned ? moveX < 0 : moveX > 0;
        isBlocking = isTurned ? moveX > 0 : moveX < 0;
        // Debug.Log("context: " + moveX);
    }

    public void SetJumpFromArduino(bool jump)
    {
        isJumpingPressed = jump;
    }
    public void SetCrouchFromArduino(bool crouch)
    {
        isCrouchPressed = crouch;
    }
    public void SetAttackFromArduino(bool attack, int level)
    {
        switch (level)
        {
            case 1:
                isLightPressed = attack;
                break;
            case 2:
                isMediumPressed = attack;
                break;
            case 3:
                isHeavyPressed = attack;
                break;
            case 0:
            default:
                isLightPressed = attack;
                isMediumPressed = attack;
                isHeavyPressed = attack;
                break;
        }
    }

    public void ResetAttackTriggers()
    {
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        animator.ResetTrigger("attack3");
    }

    private void Jump()
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

    private void Crouch()
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

    private void Block()
    {
        if (isBlocking && !isBlockAnimation)
        {
            attack.Block(true);
            animator.SetBool("isBlocking", true);
            isBlockAnimation = true;
            // to do: transition to block attack animation if being hit (trigger: blockAttack)
        }
        else if (!isBlocking && isBlockAnimation)
        {
            attack.Block(false);
            animator.SetBool("isBlocking", false);
            isBlockAnimation = false;
        }
    }

    private void Move()
    {
        if (isBlocking || isCrouching || attack.IsAttacking || player.IsDead) // we need a crouch forward animation! until then I freez crouch
        {
            characterController.Move(Vector3.zero);
        }
        else
        {
            // fixing z index if character colliders are forcing the game objects off
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
        AttackLevel level = GetAttackLevel();
        if (level == AttackLevel.NONE) {
            return;
        }
        else
        {   // standard attack
            if (IsIdle() || isJumpingPressed)
            {
                animator.SetTrigger("attack1");
                attack.StartAttack(standardAttack, level);
                animator.ResetTrigger("attack1");
                return;
            } // forward attack
            if ((isJumpingPressed && isForward) || isForward) // could be separated for two different animation!
            {
                animator.SetTrigger("attack2");
                attack.StartAttack(forwardAttack, level);
                animator.ResetTrigger("attack2");
                return;

            } // crouch attack
            if ((isForward && isCrouching) || isCrouching)  // could be separated for two different animation!
            {
                animator.SetTrigger("attack3");
                attack.StartAttack(crouchAttack, level);
                animator.ResetTrigger("attack2");
                return;
            }
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

    private void GameOver()
    {
        animator.SetTrigger("die");
        Animator opponentAnim = m_opponent.GetComponent<Animator>();
        if (opponentAnim != null) {opponentAnim.SetTrigger("win"); }
        isGameOver = true;
    }

    private AttackLevel GetAttackLevel()
    {
        if (isLightPressed)
        {
            return AttackLevel.LIGHT;
        }
        else if (isMediumPressed)
        {
            return AttackLevel.MEDIUM;
        }
        else if (isHeavyPressed)
        {
            return AttackLevel.HEAVY;
        }
        return AttackLevel.NONE;
    }

    private bool IsIdle()
    {
        return moveX == 0 && !isJumpingPressed && !isCrouchPressed && (isLightPressed || isMediumPressed || isHeavyPressed);
    }

}
