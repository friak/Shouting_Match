using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Background;
    [SerializeField]
    private GameObject m_Menu;
    [SerializeField]
    private float m_AnimationTime;

    private bool m_isOpen;
    private bool m_isMoving;
    private Vector3 m_startPosition;
    private Vector3 m_endPosition;
    private float m_distance;
    private Image image;

    private void Start()
    {
        if (m_Menu == null || m_Background == null) return;
        m_isOpen = false;
        //m_distance = m_Menu.GetComponent<RectTransform>().sizeDelta.y *m_Menu.transform.localScale.y;
        m_Menu.SetActive(false);
        image = m_Background.GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = false;
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // && !m_isMoving)
        {
            if(m_Background != null && m_Menu != null)
            {   
                m_isOpen = !m_isOpen;
                // StartCoroutine(CoAnimatePauseMenu());  // needs bugfixing!!
                if (image != null)
                {
                    image.raycastTarget = m_isOpen ? true : false;
                    image.color = m_isOpen ? new Color(image.color.r, image.color.g, image.color.b, 0.5f) : new Color(image.color.r, image.color.g, image.color.b, 0f);
                    m_Menu.SetActive(m_isOpen);
                }
                GameStateManager.Instance.TogglePause();
                Debug.Log("Pausing game ..."); //debug
            }
            
        }
    }

    /* private IEnumerator CoAnimatePauseMenu()
    {
        m_isMoving = true;
        float elapsedTime = 0;
        int direction = m_isOpen ? -1 : 1;
        m_startPosition = m_Menu.transform.localPosition;
        m_endPosition = m_startPosition - direction * new Vector3(0, m_distance, 0);

        Debug.Log("start:" + m_startPosition + ", end: " + m_endPosition);
        if (image != null)
        {
            image.raycastTarget = m_isOpen ? false : true;
            image.color = m_isOpen ? new Color(image.color.r, image.color.g, image.color.b, 0f) : new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }

        while (elapsedTime < m_AnimationTime)
        {
            m_Menu.transform.localPosition = Vector3.Lerp(m_startPosition, m_endPosition, elapsedTime / m_AnimationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_Menu.transform.position = m_endPosition;
        m_isMoving = false;
        yield return null;
    }*/

}
