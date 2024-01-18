
using UnityEngine;

public class Player : MonoBehaviour
{
    private float max_health = 100;
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
        InitPLayer(playerSA);
    }

    public void InitPLayer(CharacterScriptableAsset playerSA)
    {
        max_health = playerSA.m_health;
        curr_health = max_health;
        playerUI.SetPlayerUI(playerSA);
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

    public CombatType GetCombatType()
    {
        return playerSA.combatType;
    }

}

