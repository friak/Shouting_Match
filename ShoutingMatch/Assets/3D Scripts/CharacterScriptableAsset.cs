using UnityEngine;

public enum CombatType
{
    VOLUME,
    DURATION
    // other types later
}


[CreateAssetMenu(fileName = "New Character", menuName = "Character Asset")]
public class CharacterScriptableAsset : ScriptableObject
{
    // UI
    public string m_name;
    public Sprite m_profile;

    // Combat
    public CombatType combatType;
    public string attackLowMax;
    public string attackMediumMax;
    public string attackHighMax;

    // demage
    public float m_health;
    public float damageSmall;
    public float damageMedium;
    public float damageLarge;

    // Prefab, it has the Animator and the CharcterController on it
    public GameObject playerPrefab;  
}
