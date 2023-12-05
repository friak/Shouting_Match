using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum PlayerState
{
    IDLE,
    MOVE,
    JUMP,
    BLOCK,
    HIT,
    JUMPATTACK,
    LIGHTATTACK,
    HEAVYATTACK,
    DEAD
}

public class PlayerController2D : MonoBehaviour
{
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

    private SpriteRenderer character;
    private Rigidbody2D rbody;
    private PlayerUI healthBar;
    public bool IsDead { get; private set; }
    private bool isOnGround;
    private bool isTurned;
    public bool IsHit {get; set;}
    private bool IsAttack;
    public PlayerState State { get; private set; }
    private Player player;
    int direction = 1;
    private int data;
    private float currentHealth;
    private PlayerController2D opponentControl;


    // Start is called before the first frame update
    void Start()
    {
        IsDead = false;
        isTurned = false;
        IsHit = false;
        IsAttack = false;
        direction = -1;
        character = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        currentHealth = 100;
        opponentControl = opponent.gameObject.GetComponent<PlayerController2D>();
    }

    void FixedUpdate()
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

        if (IsDead) return;
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        
        // TURN
        if (!isTurned && transform.position.x > opponent.position.x)
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
            if(State != PlayerState.MOVE)
            {
                State = PlayerState.MOVE;
                ChangeState(State);
            }
        }
        else if (Input.GetKey(backward) || data == player.Controls["Backward"])
        {
            rbody.velocity = new Vector2(direction * moveSpeed, rbody.velocity.y);
            if (State != PlayerState.MOVE)
            {
                State = PlayerState.MOVE;
                ChangeState(State);
            }
        }
        else
        {
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            if (State != PlayerState.IDLE)
            {
                State = PlayerState.IDLE;
                ChangeState(State);
            }
        }

        //JUMP
        if ((Input.GetKeyDown(jump) || data == player.Controls["Jump"]) && isOnGround)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, jumpHeight);
            if (State != PlayerState.JUMP)
            {
                State = PlayerState.JUMP;
                ChangeState(State);
            }
        }
        // jump attack 
        if ((isTurned && (Input.GetKey(forward) || data == player.Controls["Forward"])
            || !isTurned && (Input.GetKey(backward) || data == player.Controls["Backward"]))
            && Input.GetKey(jump))
        {
            //rbody.velocity = new Vector2(rbody.velocity.x, jumpHeight);
            if (State != PlayerState.JUMPATTACK)
            {
                State = PlayerState.JUMPATTACK;
                ChangeState(State);
                if (!IsAttack) StartCoroutine(CheckHit(player.Character.damageLarge));
            }
        }

        // ATTACK (ground)
        if (Input.GetKey(attack0) || data == player.Controls["Attack1"])
        {
            if (State != PlayerState.LIGHTATTACK)
            {
                State = PlayerState.LIGHTATTACK;
                ChangeState(State);
                if(!IsAttack) StartCoroutine(CheckHit(player.Character.damageSmall));
            }
        }
        if (Input.GetKey(attack1) || data == player.Controls["Attack2"])
        {
            if (State != PlayerState.HEAVYATTACK)
            {
                State = PlayerState.HEAVYATTACK;
                ChangeState(State);
                if (!IsAttack) StartCoroutine(CheckHit(player.Character.damageLarge));
            }
        }

        // BLOCK
        if (Input.GetKey(block) || data == player.Controls["Block"])
        {
            if (State != PlayerState.BLOCK)
            {
                State = PlayerState.BLOCK;
                ChangeState(State);
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
            case PlayerState.HIT:
                {
                    character.sprite = player.Character.m_getDamage;
                    Debug.Log("HIT");
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
           case PlayerState.DEAD:
            {
                character.sprite = player.Character.m_lose;
                return;
            }
        }
    }

    private IEnumerator CheckHit(int damage)
    {
        IsAttack = true;
        if (opponentControl.State != PlayerState.BLOCK && IsHit)
        {
            opponentControl.State = PlayerState.HIT;
            opponentControl.TakeDamage(damage);
        }
        yield return new WaitForSecondsRealtime(1f);
        IsAttack = false;
    }
         

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Damage taken, health: " + currentHealth); ;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            State = PlayerState.DEAD;
            ChangeState(State);
            healthBar.GameOver.text += "\n" + opponentControl.character.name + " WINS!";
            healthBar.GameOver.gameObject.SetActive(true);
            IsDead = true;
        }
    }

     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            opponentControl.IsHit = true;
            Debug.Log("hit");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            opponentControl.IsHit = false;
            Debug.Log("hit exit");
        }
    }

    public void SetPlayer(Player p)
    {
        player = p;
        character.sprite = player.Character.m_idle;
    }

    public void SetHealthBar(PlayerUI hb, int max)
    {
        healthBar = hb;
        healthBar.SetMaxValue(max);
        currentHealth = max;
        Debug.Log("max health: " + currentHealth);
    }

    public void FlipCharacter()
    {
        character.transform.localScale = new Vector3(-1, 1, 1);
        direction = 1;
    }
}