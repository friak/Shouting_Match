using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack Asset")]
public class AttackScriptableAsset : ScriptableObject
{
    public AttackType attackType;

    public GameObject lightAttackPrefab;
    public GameObject mediumAttackPrefab;
    public GameObject heavyAattackPrefab;
    public GameObject chargePrefab;
    public GameObject indicatorPrefab;
    public int lightDamage;
    public int mediumDamage;
    public int heavytDamage;
}
