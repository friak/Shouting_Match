using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer character;
    public bool IsDead { get; private set; }
    public bool IsFlipped { get; private set; }

    [SerializeField]
    private GameObject opponent;
    [SerializeField]
    private KeyCode forward, block, jump, crouch, attack0, attack1, attack2;
    [SerializeField]
    private float jumpHeight, moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        IsDead = false;
        IsFlipped = false;
        character = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(!IsFlipped && transform.position.x > opponent.transform.position.x)
        {
            IsFlipped = !IsFlipped;
            character.transform.RotateAround(transform.position, transform.up, 180f);
        }
    }

    public void SetCharacter(Sprite playerSprite)
    {
        character.sprite = playerSprite;
    }
}
