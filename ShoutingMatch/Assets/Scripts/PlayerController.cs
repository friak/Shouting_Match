using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;

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
    //Choosing USB port for Player 1
    public SerialPort sp = new SerialPort("COM3", 9600);
    //Arduino data
    public int data;

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
    private bool isChanging;
    private PlayerState state;
    private CharacterSO playerSO;

    // Start is called before the first frame update
    void Start()
    {
        // Arduino Communication
        try
        {
            sp.Open();
            sp.ReadTimeout = 25;
        }
        catch (System.Exception)
        {
            Debug.Log("Port Not Found!");
        }

        IsDead = false;
        isTurned = false;
        isChanging = false;
        direction = -1;
        character = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Arduino Communication
        if (sp.IsOpen)
        {
            try
            {
                data = sp.ReadByte();
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
        if (Input.GetKey(forward))
        {
            rbody.velocity = new Vector2(direction * -moveSpeed, rbody.velocity.y);
            if(state != PlayerState.MOVE)
            {
                state = PlayerState.MOVE;
            }
        }
        else if (Input.GetKey(backward))
        {
            rbody.velocity = new Vector2(direction * moveSpeed, rbody.velocity.y);
            if (state != PlayerState.MOVE)
            {
                state = PlayerState.MOVE;
            }
        }
        else
        {
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            if (state != PlayerState.IDLE)
            {
                state = PlayerState.IDLE;
            }
        }
        //JUMP
        if (Input.GetKeyDown(jump) && isOnGround)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, jumpHeight);
            if (state != PlayerState.JUMP)
            {
                state = PlayerState.JUMP;
            }
        }
        // jump attack 
        if ((isTurned && Input.GetKey(jump) || !isTurned && Input.GetKey(backward)) && Input.GetKey(forward))
        {
            //rbody.velocity = new Vector2(rbody.velocity.x, jumpHeight);
            if (state != PlayerState.JUMPATTACK)
            {
                state = PlayerState.JUMPATTACK;
            }
            //check for hit
        }

        // ATTACK (ground)
        if (Input.GetKey(attack0))
        {
            if (state != PlayerState.LIGHTATTACK)
            {
                state = PlayerState.LIGHTATTACK;
            }
            //check for hit
        }
        if (Input.GetKey(attack1))
        {
            if (state != PlayerState.HEAVYATTACK)
            {
                state = PlayerState.HEAVYATTACK;
            }
            //check for hit
        }

        // BLOCK
        if (Input.GetKey(block))
        {
            if (state != PlayerState.BLOCK)
            {
                state = PlayerState.BLOCK;
            }
        }

        switch (state)
        {
            case PlayerState.IDLE:
                {
                    character.sprite = playerSO.m_annoy;
                    return;
                }
            case PlayerState.MOVE:
                {
                    // character.sprite = playerSO.move; // missing sprite
                    character.sprite = playerSO.m_idle;
                    return;
                }
            case PlayerState.JUMP:
                {
                    // character.sprite = playerSO.jump; // missing sprite
                    character.sprite = playerSO.m_idle;
                    return;
                }
            case PlayerState.JUMPATTACK:
                {
                    character.sprite = playerSO.m_jumpAttack;
                    return;
                }
            case PlayerState.BLOCK:
                {
                    // character.sprite = playerSO.m_block; // missing sprite
                    character.sprite = playerSO.m_idle;
                    return;
                }
            case PlayerState.LIGHTATTACK:
                {
                    character.sprite = playerSO.m_lightAttack;
                    return;
                }
            case PlayerState.HEAVYATTACK:
                {
                    character.sprite = playerSO.m_heavyAttack;
                    return;
                }
        }
    }

    private IEnumerator SetSprite(Sprite sprite)
    {
        isChanging = true;
        character.sprite = sprite;
        yield return new WaitForSeconds(3f);
        isChanging = false;
    }

    public void SetCharacter(CharacterSO pSO)
    {
        playerSO = pSO;
        character.sprite = pSO.m_idle;
    }
    public void FlipCharacter()
    {
        character.transform.localScale = new Vector3(-1, 1, 1);
        direction = 1;
    }
}
