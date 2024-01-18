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
    private GameObject blockPrefab;
    private AttackScriptableAsset currentAttack;
    private AttackLevel attackLevel;
    private float damage;
    private AttackState state;
    private GameObject opponent;
    private Player opponentPlayer;
    private PlayerController opponentController;

    private GameObject attackInstance;
    private GameObject chargeInstance;
    private GameObject indicatorInstance;
    private GameObject blockInstance;

    private bool isEntering = false;
    private bool isExiting = false;
    private bool didHitOnce = false;

    private float damageDistance = 2.2f;
    public bool IsAttacking { get; private set; } = false;
    Vector3 opponentOriginalPos;
    

    private void Start()
    {
        state = AttackState.DORMANT;
        opponent = GetComponentInParent<PlayerController>().Opponent.gameObject;
        opponentOriginalPos = opponent.transform.position;
        opponentPlayer = opponent.GetComponentInParent<Player>();
        opponentController = opponent.GetComponent<PlayerController>();
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
        damageDistance = currentAttack.attackType == AttackType.BLASTSELF ? 4f : 2f;
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
                        chargeInstance = Instantiate(currentAttack.chargePrefab, transform);
                        StartCoroutine(CoAttack(transform.position + new Vector3(0, -2.5f, 0), transform.rotation, transform));
                    }
                    else
                    {
                        if (currentAttack.indicatorPrefab != null)
                        {
                            indicatorInstance = Instantiate(currentAttack.indicatorPrefab, transform.position + new Vector3(0, 0.5f, -0.5f), transform.rotation, transform);
                            Debug.Log("Indicator instantiated");
                            StartCoroutine(CoShowIndicator());
                        }
                        attackInstance = InstantiateAttack(transform.position + new Vector3(0, -2.5f, 0), transform.rotation, transform);
                        // CheckForHit();
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
                            chargeInstance = Instantiate(currentAttack.chargePrefab, transform.position, transform.rotation, transform);
                            StartCoroutine(CoAttack(opponent.transform.position, opponent.transform.rotation, opponent.transform));
                        }
                        else
                        {
                            StartCoroutine(CoAttack(opponent.transform.position, opponent.transform.rotation, opponent.transform));
                        }
                    }
                    break;
                }
            case AttackType.PROJECTILE:
                {
                    float offset = transform.position.x < 0 ? -1.2f : 1.2f; // start projectile from behind even if turned
                    StartCoroutine(CoAttack(transform.position + new Vector3(offset, 0, 0), transform.rotation, transform));
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
                    float step = 9.5f * Time.deltaTime; //  distance to move
                    attackInstance.transform.position = Vector3.MoveTowards(attackInstance.transform.position, opponentOriginalPos + new Vector3(0, -1.8f,0), step);
                    if (Mathf.Abs(attackInstance.transform.position.x - opponent.transform.position.x) < 0.5f)
                    {
                        damage = opponentController.IsBlocking ? damage / 4 : damage;
                        opponentPlayer.TakeDamage(damage);
                        Debug.Log("PROJECTILE DAMAGE: " + damage);
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

    private IEnumerator CoAttack(Vector3 position, Quaternion rotation, Transform trans)
    {
        yield return new WaitForSeconds(1f);
        Destroy(chargeInstance);
        Debug.Log("Charge over");
        if (currentAttack.indicatorPrefab != null)
        {
            indicatorInstance = Instantiate(currentAttack.indicatorPrefab, opponent.transform.position, rotation, trans);
            Debug.Log("Indicator instantiated (in co-charge)");
            StartCoroutine(CoShowIndicator());
            yield return false;
        }
        attackInstance = InstantiateAttack(position, rotation, trans);
        Debug.Log("Attack instantiated");
        state = AttackState.UPDATE;
        isEntering = false;
        yield return true;
    }

    private IEnumerator CoExitAttack()
    {
        yield return new WaitForSeconds(1f);
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
        // Debug.Log("++++++ Checking hit +++++++++++ \n " + attackInstance.transform.localPosition.x + " " + opponent.position.x);
        if (Mathf.Abs(attackInstance.transform.position.x - opponent.transform.position.x) < damageDistance && !didHitOnce)
        {
            damage = opponentController.IsBlocking ? damage / 4 : damage;
            opponentPlayer.TakeDamage(damage);
            didHitOnce = true;
            Debug.Log("DAMAGE: " + damage);
        }
    }

    private GameObject InstantiateAttack(Vector3 position, Quaternion rotation, Transform trans)
    {
        switch (attackLevel)
        {
            default:
            case AttackLevel.LIGHT:
                {
                    return Instantiate(currentAttack.lightAttackPrefab, position, rotation, trans);
                }
            case AttackLevel.MEDIUM:
                {
                    return Instantiate(currentAttack.mediumAttackPrefab, position, rotation, trans);
                }
            case AttackLevel.HEAVY:
                {
                    return Instantiate(currentAttack.heavyAattackPrefab, position, rotation, trans);
                }
        }
    }

    public void Block(bool isBlocking)
    {
        if (isBlocking)
        {
            float offset = transform.position.x < 0 ? 1.2f : -1.2f; // position the shield in front of the player, even when turned
            float rotateY = transform.position.x < 0 ? 0f : 90f; // flip the shield when turned
            blockInstance = Instantiate(blockPrefab, transform.position + new Vector3(offset, 0, 0), transform.rotation, transform);
            blockInstance.transform.GetChild(0).gameObject.transform.Rotate(new Vector3(0, rotateY, 0));
            // Debug.Log("SHIELD ON");
        }
        else
        {
            Destroy(blockInstance);
        }
    }
}
