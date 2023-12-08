using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightingUI : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableAsset playerAsset;
    
    private GameObject profile;
    private TextMeshProUGUI characterName;
    private TextMeshProUGUI healthPoints;

    private void Start()
    {
        // for the live game this has to be commented out
        profile.GetComponent<RawImage>().texture = playerAsset.m_profile.texture;
        characterName.text = playerAsset.name;
        healthPoints.text = playerAsset.m_health.ToString();
    }
    public void SetHealthText(string health)
    {
        healthPoints.text = health;
    }

    public void SetPlayerUI(CharacterScriptableAsset asset)
    {
        profile.GetComponent<RawImage>().texture = asset.m_profile.texture;
        characterName.text = asset.name;
        healthPoints.text = asset.m_health.ToString();
    }
}
