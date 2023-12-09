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
    private float health;

    [SerializeField]
    private GameObject profileImage;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image fill;



    private void Start()
    {
        // for the live game this has to be commented out
        profileImage.GetComponent<Image>().sprite = image;
        characterName.text = ch_name;
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetPlayerUI(CharacterScriptableAsset asset)
    {
        profileImage.GetComponent<RawImage>().texture = asset.m_profile.texture;
        characterName.text = asset.name;
    }

    public void ReduceHealthBar(float new_health)
    {
        slider.value = new_health;
    }
}
