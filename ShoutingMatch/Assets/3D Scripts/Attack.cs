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
public enum AttackLevel
{
    NONE,
    LIGHT,
    MEDIUM,
    HEAVY
}
public class Attack : MonoBehaviour
{
    [SerializeField]
    private GameObject attackParent;
    private AttackScriptableAsset currentAttack;
    private AttackLevel attackLevel;
    private int damage;
    private AttackState state;
    private Transform opponent;

    private GameObject attackInstance;
    private GameObject chargeInstance;
    private GameObject indicatorInstance;

    private bool isEntering = false;
    private bool isExiting = false;
    private bool didHitOnce = false;

    private float damageDistance = 2.2f; // change this later to custom distance for MLH
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

    public void StartAttack(AttackScriptableAsset attack, AttackLevel level)
    {
        currentAttack = attack;
        attackLevel = level;
        if(level == AttackLevel.LIGHT) { damage = currentAttack.lightDamage; }
        if(level == AttackLevel.MEDIUM) { damage = currentAttack.mediumDamage; }
        if(level == AttackLevel.HEAVY) { damage = currentAttack.heavytDamage; }
        IsAttacking = true;
        state = AttackState.ENTER;
    }
    private void EnterAttack()
    {
        isEntering = true;
        Debug.Log("Entered attack");
        state = AttackState.DORMANT; //waiting until enter is done
        switch (currentAttack.attackType)
        {
            case AttackType.BLASTSELF:
                {
                    if (currentAttack.chargePrefab != null)
                    {
                        chargeInstance = Instantiate(currentAttack.chargePrefab, attackParent.transform);
                        StartCoroutine(CoCharge(attackParent.transform));
                    }
                    else
                    {
                        /*if (currentAttack.indicatorPrefab != null)
                        {
                            indicatorInstance = Instantiate(currentAttack.indicatorPrefab, attackParent.transform);
                            Debug.Log("Indicator instantiated");
                            StartCoroutine(CoShowIndicator());
                        }*/
                        attackInstance = InstantiateAttack(attackParent.transform); 
                        CheckForHit();
                        state = AttackState.UPDATE;
                        isEntering = false;
                    }
                    break;
                }
            case AttackType.BLASTOPPONENT:
                {
                    if (opponent != null)
                    {
                        if (currentAttack.chargePrefab != null)
                        {
                            chargeInstance = Instantiate(currentAttack.chargePrefab, opponent); // this is not a character charge but an indicator of the hit location
                            StartCoroutine(CoCharge(opponent));
                        }
                        else
                        {
                            StartCoroutine(CoCharge(attackParent.transform));
                        }
                    }
                    break;
                }
            case AttackType.PROJECTILE:
                {
                    StartCoroutine(CoCharge(attackParent.transform));
                    break;
                }
        }

    }

    public void ExecuteAttack()
    {
        Debug.Log("Executing attack");
        switch (currentAttack.attackType)
        {
            case AttackType.BLASTSELF:
            case AttackType.BLASTOPPONENT:
                {
                    CheckForHit();
                    state = AttackState.EXIT;
                    break;
                }
            case AttackType.PROJECTILE:
                {
                    // thorw it at the opponent
                    float step = 8f * Time.deltaTime; //  distance to move
                    attackInstance.transform.position = Vector3.MoveTowards(attackInstance.transform.position, opponent.position + new Vector3(0, 2.8f,0), step);
                    // CheckForHit();
                    if (Mathf.Abs(attackInstance.transform.position.x - opponent.position.x) < 0.3f)
                    {
                        Player opp = opponent.GetComponentInParent<Player>();
                        opp.TakeDamage(damage);
                        Debug.Log("-------- Health after projectile hit: " + opp.Health);
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
        yield return new WaitForSeconds(1f);
        Destroy(chargeInstance);
        Debug.Log("Charge over");
        /*if (currentAttack.indicatorPrefab != null)
        {
            indicatorInstance = Instantiate(currentAttack.indicatorPrefab, opponent);
            Debug.Log("Indicator instantiated (in co-charge)");
            StartCoroutine(CoShowIndicator());
            yield return false;
        }*/
        attackInstance = InstantiateAttack(trans);
        Debug.Log("Attack instantiated");
        state = AttackState.UPDATE;
        isEntering = false;
        yield return true;
    }

    private IEnumerator CoExitAttack()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Exiting");
        Destroy(attackInstance);
        IsAttacking = false;
        didHitOnce = false;
        isExiting = false;
        yield return true;
    }

    private IEnumerator CoShowIndicator()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Showingindicator");
        Destroy(indicatorInstance);
        yield return true;
    }

    private void CheckForHit()
    {
        Debug.Log("++++++ Checking hit +++++++++++ \n " + attackInstance.transform.localPosition.x + " " + opponent.position.x);
        if (Mathf.Abs(attackInstance.transform.localPosition.x - opponent.position.x) < damageDistance && !didHitOnce)
        {
            Player opp = opponent.GetComponentInParent<Player>();
            opp.TakeDamage(damage);
            didHitOnce = true;
            Debug.Log("-------- Health after hit: " + opp.Health);
        }
    }

    private GameObject InstantiateAttack(Transform trans)
    {
        switch (attackLevel)
        {
            default:
            case AttackLevel.LIGHT:
                {
                    return Instantiate(currentAttack.lightAttackPrefab, trans);
                }
            case AttackLevel.MEDIUM:
                {
                    return Instantiate(currentAttack.mediumAttackPrefab, trans);
                }
            case AttackLevel.HEAVY:
                {
                    return Instantiate(currentAttack.heavyAattackPrefab, trans);
                }
        }
    }
}
