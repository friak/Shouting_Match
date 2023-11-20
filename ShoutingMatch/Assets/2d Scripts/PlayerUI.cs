using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Image profileImg;

    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    public Slider HealthBar;

    [SerializeField]
    public TextMeshProUGUI GameOver;

    public void SetMaxValue(float max)
    {
        HealthBar.maxValue = max;
        HealthBar.value = max;
    }

    public void SetHealth(float health)
    {
        HealthBar.value = health;
    }

    public void SetProfile(Sprite img)
    {
        profileImg.sprite = img;
    }

    public void SetName(string name)
    {
        playerName.text = name;
    }

    public void Flip()
    {
        transform.Rotate(transform.up, 180f);
        playerName.transform.Rotate(transform.up, 180f);
        playerName.alignment = TextAlignmentOptions.MidlineRight;
    }
}
