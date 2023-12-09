using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightingUI : MonoBehaviour
{
    // these are only exposed used for testing purposes
    [SerializeField]
    private Sprite image;
    [SerializeField]
    private string ch_name;
    [SerializeField]
    private string health;

    [SerializeField]
    private GameObject profileImage;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private TextMeshProUGUI healthPoints;



    private void Start()
    {
        // for the live game this has to be commented out
        profileImage.GetComponent<Image>().sprite = image;
        characterName.text = ch_name;
        healthPoints.text = health;
    }
    public void SetHealthText(string health)
    {
        healthPoints.text = health;
    }

    public void SetPlayerUI(CharacterScriptableAsset asset)
    {
        profileImage.GetComponent<RawImage>().texture = asset.m_profile.texture;
        characterName.text = asset.name;
        healthPoints.text = asset.m_health.ToString();
    }
}
