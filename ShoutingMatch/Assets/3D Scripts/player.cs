
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health = 100;
    private float lightDamage;
    private float mediumDamage;
    private float heavyDamage;

    public bool IsPlayer1 { get; private set; }
    public bool IsDead { get;  private set; }
    public float Health { get { return health; } private set { } }
    // private int speed;
    // private float blockTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitPLayer(CharacterScriptableAsset asset)
    {
        health = asset.m_health;
        lightDamage = asset.damageSmall;
        mediumDamage = asset.damageMedium;
        heavyDamage = asset.damageLarge;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Damage taken, health: " + health); ;
        if (health <= 0)
        {
            IsDead = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Debug.Log("hit by player");
        }
        // blasts and projectiles must be tagged as Attack
        if (other.gameObject.tag == "Attack") 
        {

            Debug.Log("hit by projectile");
        }
    }

    /*void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Debug.Log("hit exit");
        }
    }*/

}

