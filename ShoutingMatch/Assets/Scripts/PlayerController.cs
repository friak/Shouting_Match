using UnityEngine;

public enum PlayerState
{
    IDLE,
    MOVE,
    JUMP,
    BLOCK,
    JUMPATTACK,
    LIGHTATTACK,
    HEAVYATTACK
}

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer character;
    private Rigidbody2D rbody;
    public bool IsDead { get; private set; }
    int direction = 1;
    [SerializeField]
    private Transform opponent;
    [SerializeField]
    private KeyCode forward, backward, jump, block, attack0, attack1;
    [SerializeField]
    private float jumpHeight, moveSpeed, groundRadius;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform groundCheck;
    private bool isOnGround;
    private bool isTurned;
    // private bool isChanging;
    private PlayerState state;
    private Player player;

    private int data;

    // Start is called before the first frame update
    void Start()
    {
        IsDead = false;
        isTurned = false;
        // isChanging = false;
        direction = -1;
        character = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Arduino Communication
        if (player.SPort.IsOpen)
        {
            try
            {
                data = player.SPort.ReadByte();
                Debug.Log("Received data: player name: " + player.Character.m_name + ", data: " + data.ToString());
            }
            catch (System.Exception)
            {
            }
        }

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // TURN
        if(!isTurned && transform.position.x > opponent.position.x)
        {
            character.transform.localScale = new Vector3(-1, 1, 1);
            isTurned = !isTurned;
        }
        else if (isTurned && transform.position.x < opponent.position.x)
        {
            character.transform.localScale = new Vector3(1, 1, 1);
            isTurned = !isTurned;
        }

        // MOVE
        if (Input.GetKey(forward) || data == player.Controls["Forward"])
        {
            rbody.velocity = new Vector2(direction * -moveSpeed, rbody.velocity.y);
            if(state != PlayerState.MOVE)
            {
                state = PlayerState.MOVE;
                ChangeState(state);
            }
        }
        else if (Input.GetKey(backward) || data == player.Controls["Backward"])
        {
            rbody.velocity = new Vector2(direction * moveSpeed, rbody.velocity.y);
            if (state != PlayerState.MOVE)
            {
                state = PlayerState.MOVE;
                ChangeState(state);
            }
        }
        else
        {
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            if (state != PlayerState.IDLE)
            {
                state = PlayerState.IDLE;
                ChangeState(state);
            }
        }
        //JUMP
        if ((Input.GetKeyDown(jump) || data == player.Controls["Jump"]) && isOnGround )
        {
            rbody.velocity = new Vector2(rbody.velocity.x, jumpHeight);
            if (state != PlayerState.JUMP)
            {
                state = PlayerState.JUMP;
                ChangeState(state);
            }
        }
        // jump attack 
        if ((isTurned && (Input.GetKey(forward) || data == player.Controls["Forward"])
            || !isTurned && (Input.GetKey(backward) || data == player.Controls["Backward"]))
            && Input.GetKey(jump))
        {
            //rbody.velocity = new Vector2(rbody.velocity.x, jumpHeight);
            if (state != PlayerState.JUMPATTACK)
            {
                state = PlayerState.JUMPATTACK;
                ChangeState(state);
            }
            //check for hit
        }

        // ATTACK (ground)
        if (Input.GetKey(attack0) || data == player.Controls["Attack1"])
        {
            if (state != PlayerState.LIGHTATTACK)
            {
                state = PlayerState.LIGHTATTACK;
                ChangeState(state);
            }
            //check for hit
        }
        if (Input.GetKey(attack1) || data == player.Controls["Attack2"])
        {
            if (state != PlayerState.HEAVYATTACK)
            {
                state = PlayerState.HEAVYATTACK;
                ChangeState(state);
            }
            //check for hit
        }

        // BLOCK
        if (Input.GetKey(block) || data == player.Controls["Block"])
        {
            if (state != PlayerState.BLOCK)
            {
                state = PlayerState.BLOCK;
                ChangeState(state);
            }
        }
        
    }

    private void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                {
                    character.sprite = player.Character.m_annoy;
                    Debug.Log("IDLE");
                    return;
                }
            case PlayerState.MOVE:
                {
                    // character.sprite = playerSO.move; // missing sprite
                    character.sprite = player.Character.m_idle;
                    Debug.Log("MOVE");
                    return;
                }
            case PlayerState.JUMP:
                {
                    // character.sprite = playerSO.jump; // missing sprite
                    character.sprite = player.Character.m_idle;
                    Debug.Log("JUMP");
                    return;
                }
            case PlayerState.JUMPATTACK:
                {
                    character.sprite = player.Character.m_jumpAttack;
                    Debug.Log("JUMP ATTACK");
                    return;
                }
            case PlayerState.BLOCK:
                {
                    // character.sprite = playerSO.m_block; // missing sprite
                    character.sprite = player.Character.m_idle;
                    Debug.Log("BLOCK");
                    return;
                }
            case PlayerState.LIGHTATTACK:
                {
                    character.sprite = player.Character.m_lightAttack;
                    return;
                }
            case PlayerState.HEAVYATTACK:
                {
                    character.sprite = player.Character.m_heavyAttack;
                    return;
                }
        }
    }
    /* private IEnumerator SetSprite(Sprite sprite)
    {
        isChanging = true;
        character.sprite = sprite;
        yield return new WaitForSeconds(3f);
        isChanging = false;
    } */


    public void SetPlayer(Player p)
    {
        player = p;
        character.sprite = player.Character.m_idle;
    }

    public void FlipCharacter()
    {
        character.transform.localScale = new Vector3(-1, 1, 1);
        direction = 1;
    }
}