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



    private void Start()
    {
        // for the live game this has to be commented out
        if (image != null) profileImage.GetComponent<Image>().sprite = image;
        characterName.text = ch_name;
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetPlayerUI(CharacterScriptableAsset asset)
    {
        profileImage.GetComponent<Image>().sprite = asset.m_profile;
        characterName.text = asset.m_name;
        slider.maxValue = asset.m_health;
        slider.value = asset.m_health;
    }

    public void ReduceHealthBar(float new_health)
    {
        slider.value = new_health;
    }
}
