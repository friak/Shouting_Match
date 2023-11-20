using UnityEngine;
using UnityEngine.UI;
 
// Character select item
public class OptionButton : MonoBehaviour
{
    private int id;
    [SerializeField]
    private Image frame, profile;
    [SerializeField]
    private Sprite spriteMultiselect, spriteP1, spriteP2;

    private CharacterSO character;
    private int numberOfSelects;

    public void SetCharacter(CharacterSO ch, int id)
    {
        this.id = id;
        character = ch;
        numberOfSelects = 0;
        profile.sprite = ch.m_profile;
        profile.gameObject.SetActive(true);
        frame.gameObject.SetActive(false);
    }


    public int GetId()
    {
        return id;
    }

    public CharacterSO GetScriptableObject()
    {
        return character;
    }

    public void SetFrame(Sprite sp)
    {
        frame.sprite = sp;
    }

    public int GetNumberOfSelects()
    {
        return numberOfSelects;
    }

    public void Select(int pID)
    {

        numberOfSelects++;
        if (numberOfSelects > 1)
        {
            numberOfSelects = 2; // cannot be more than 2 in our case
            frame.sprite = spriteMultiselect;
        }
        if(numberOfSelects == 1)
        {
            frame.sprite = pID == 1 ? spriteP1 : spriteP2;
            frame.gameObject.SetActive(true);
        }
    }

    public void Deselect(int pID)
    {
        
        if (numberOfSelects > 0) // in any other cases we cannot deselect
        {
            numberOfSelects--;
            frame.sprite = pID == 1 ? spriteP2 : spriteP1;
            if (numberOfSelects == 0)
            {
                frame.gameObject.SetActive(false);
            }
        }

    }

}
