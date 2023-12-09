using UnityEngine;
using System.Collections;

public enum AttackType
{
    BLASTSELF,
    BLASTOPPONENT,
    PROJECTILE
}

public enum AttackState
{
    DORMANT,
    ENTER,
    UPDATE,
    EXIT
}
public class Attack : MonoBehaviour
{
    [SerializeField]
    private AttackType attackType;
    public AttackType AttackType { get { return attackType; } private set { } }
    [SerializeField]
    private GameObject attackPrefab;
    [SerializeField]
    private GameObject chargePrefab;
    [SerializeField]
    private int damage;
    // multipliers for damage: 2.5 * light = medium, 4 * light = heavy // or ScriptableAssets!!!
    private AttackState state;
    private Transform opponent;
    private GameObject attackInstance;
    private GameObject chargeInstance;

    private bool isEntering = false;
    private bool isExiting = false;
    public bool IsAttacking { get; private set; } = false;



    private void Start()
    {
        state = AttackState.DORMANT;
        opponent = GetComponentInParent<PlayerController>().Opponent;
    }

    private void Update()
    {
        switch (state)
        {
            case AttackState.DORMANT:
                {
                    return;
                }
            case AttackState.ENTER:
                {
                    if (!isEntering) EnterAttack();
                    return;
                }
            case AttackState.UPDATE:
                {
                    ExecuteAttack();
                    return;
                }
            case AttackState.EXIT:
                {
                    if (!isExiting) ExitAttack();
                    return;
                }
        }
    }

    public void StartAttack()
    {
        IsAttacking = true;
        state = AttackState.ENTER;
    }
    private void EnterAttack()
    {
        isEntering = true;
        Debug.Log("Entered attack");
        state = AttackState.DORMANT; //waiting until enter is done
        switch (attackType)
        {
            case AttackType.BLASTSELF:
                {
                    if (chargePrefab != null)
                    {
                        chargeInstance = Instantiate(chargePrefab, transform);
                        StartCoroutine(CoCharge(transform));
                    }
                    else
                    {
                        attackInstance = Instantiate(attackPrefab, transform);
                        state = AttackState.UPDATE;
                        isEntering = false;
                    }
                    break;
                }
            case AttackType.BLASTOPPONENT:
                {
                    if (opponent != null)
                    {
                        if (chargePrefab != null)
                        {
                            chargeInstance = Instantiate(chargePrefab, opponent); // this is not a character charge but an indicator of the hit location
                            StartCoroutine(CoCharge(opponent));
                        }
                        else
                        {
                            attackInstance = Instantiate(attackPrefab, opponent);
                            state = AttackState.UPDATE;
                            isEntering = false;
                        }
                    }
                    break;
                }
            case AttackType.PROJECTILE:
                {
                    attackInstance = Instantiate(attackPrefab, transform);
                    state = AttackState.UPDATE;
                    isEntering = false;
                    break;
                }
        }
    }

    public void ExecuteAttack()
    {
        Debug.Log("Executing attack");
        switch (attackType)
        {
            case AttackType.BLASTSELF:
            case AttackType.BLASTOPPONENT:
                {
                    state = AttackState.EXIT;
                    break;
                }
            case AttackType.PROJECTILE:
                {
                    // thorw it at the opponent
                    float step = 5f * Time.deltaTime; //  distance to move
                    attackInstance.transform.position = Vector3.MoveTowards(attackInstance.transform.position, opponent.position, step);
                    // Debug.Log(Mathf.Abs(attackInstance.transform.position.x - opponent.position.x) + "");
                    if (Mathf.Abs(attackInstance.transform.position.x - opponent.position.x) < 0.5f)
                    {
                        state = AttackState.EXIT;
                    }
                    break;
                }
        }
    }

    public void ExitAttack()
    {
        isExiting = true;
        state = AttackState.DORMANT;
        StartCoroutine(CoExitAttack());
    }

    private IEnumerator CoCharge(Transform trans)
    {
        Debug.Log("CoCharge 1");
        yield return new WaitForSeconds(2f);
        Destroy(chargeInstance);
        Debug.Log("CoCharge 2");
        attackInstance = Instantiate(attackPrefab, trans);
        state = AttackState.UPDATE;
        isEntering = false;
        yield return true;
    }

    private IEnumerator CoExitAttack()
    {
        Debug.Log("Exiting start");
        yield return new WaitForSeconds(1f);
        Debug.Log("Exiting");
        Destroy(attackInstance);
        IsAttacking = false;
        isExiting = false;
        yield return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = opponent.gameObject.tag;
        if (other.tag == tag)
        {
            Player opp = opponent.GetComponentInParent<Player>();
            opp.TakeDamage(damage);
            Debug.Log("-------- Health after hit: " + opp.Health);
        }
    }
}
