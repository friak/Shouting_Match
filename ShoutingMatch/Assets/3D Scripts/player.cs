
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private string char_name = "Sybil";
    private float max_health = 100;
    private Image profile;
    private float lightDamage = 10;
    private float mediumDamage = 15;
    private float heavyDamage = 20;
    private float curr_health;

    public bool IsPlayer1 { get; private set; }
    public bool IsDead { get;  private set; }
    public float Health { get { return curr_health; } private set { } }
    // private int speed;
    // private float blockTime;

    [SerializeField]
    private PlayerFightingUI playerUI;

    // for testing purposes, the gamesStateManager should set the player
    [SerializeField]
    private CharacterScriptableAsset playerSA; 


    void Start()
    {
        // this will be set in the InitPLayer method
        char_name = playerSA.m_name;
        max_health = playerSA.m_health;
        profile.sprite = playerSA.m_profile;
        lightDamage = playerSA.damageSmall;
        mediumDamage = playerSA.damageMedium;
        heavyDamage = playerSA.damageLarge;
        curr_health = max_health;

        playerUI.SetPlayerUI(playerSA);

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void InitPLayer(CharacterScriptableAsset playerSA)
    {
        char_name = playerSA.m_name;
        max_health = playerSA.m_health;
        profile.sprite = playerSA.m_profile;
        lightDamage = playerSA.damageSmall;
        mediumDamage = playerSA.damageMedium;
        heavyDamage = playerSA.damageLarge;
        curr_health = max_health;
    }

    public void TakeDamage(float damage)
    {
        curr_health -= damage;
        playerUI.ReduceHealthBar(curr_health);
        Debug.Log("Damage taken, health: " + curr_health); ;
        if (curr_health <= 0)
        {
            IsDead = true;
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hit by player");
        }
        // blasts and projectiles must be tagged as Attack
        if (other.tag == "Attack") 
        {

            Debug.Log("hit by attack");
        }
    }*/

}

