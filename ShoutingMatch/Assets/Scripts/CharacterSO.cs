using UnityEngine;

[CreateAssetMenu(fileName="New Character", menuName="Character" )]
public class CharacterSO : ScriptableObject
{
    public string m_name;
    public Sprite m_profile;

    public int m_health;
    public int damageSmall;
    public int damageLarge;

    public Sprite m_annoy;
    public Sprite m_idle;
    // missing sprites ...
    // public Sprite m_block;
    // public Sprite m_move;
    // public Sprite m_jump;
    public Sprite m_getDamage;
    public Sprite m_lose;
    public Sprite m_jumpAttack;
    public Sprite m_lightAttack;
    public Sprite m_heavyAttack;

}
