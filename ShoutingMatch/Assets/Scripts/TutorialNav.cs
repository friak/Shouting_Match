using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNav : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_slides;
    [SerializeField]
    private Button m_leftButton;
    [SerializeField]
    private Button m_rightButton;
    private int m_current;

    void Start()
    {
        if (m_slides != null)
        {
            m_current = 0;
            foreach (GameObject go in m_slides)
            {
                go.SetActive(false);
            }
            m_slides[m_current].SetActive(true);
            m_rightButton.interactable = false;
        }
    }

    public void OnNextClick()
    {
        Slide(true);
    }

    public void OnPrevClick()
    {
        Slide(false);
    }

    private void Slide(bool next)
    {
        if (next) // slide left
        {
            if(m_current + 1 <= m_slides.Count)
            {
                m_slides[m_current+1].SetActive(true);
                m_slides[m_current].SetActive(false);
                Debug.Log("Left to: " + m_current);
                m_current++;
                if (m_current == m_slides.Count - 1)
                {
                    m_leftButton.interactable = false;
                }
                else if(m_current == 1)
                {
                    m_rightButton.interactable = true;
                }
            }
        }
        else // slide right
        {
            if (m_current - 1 >= 0)
            {
                m_slides[m_current - 1].SetActive(true);
                m_slides[m_current].SetActive(false);
                Debug.Log("Right to: " + m_current);
                m_current--;
                if (m_current == 0)
                {
                    m_rightButton.interactable = false;
                }
                else if (m_current == m_slides.Count - 2)
                {
                    m_leftButton.interactable = true;
                }
            }
        }
    }
}
