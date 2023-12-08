using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Asset")]
public class CharacterScriptableAsset : ScriptableObject
{
    // UI
    public string m_name;
    public Sprite m_profile;

    // demage
    public float m_health;
    public float damageSmall;
    public float damageMedium;
    public float damageLarge;

    public GameObject playerPrefab;  // it has the Animator and the CharcterController on it

}
