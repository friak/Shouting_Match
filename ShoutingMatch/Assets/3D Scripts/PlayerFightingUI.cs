using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject profileImage;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private Slider slider;


    private void Start()
    {

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
