using UnityEngine;

[CreateAssetMenu(fileName="New Character", menuName="Character" )]
public class CharacterSO : ScriptableObject
{
    public string m_name;
    public Sprite m_profile;
    public Sprite m_idle;
    public int m_health;
    public int damageSmall;
    public int damageLarge;


    /// other fields, e.g. attack types, animations, musics??

}
