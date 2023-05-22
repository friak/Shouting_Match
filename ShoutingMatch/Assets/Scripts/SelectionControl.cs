using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

// Character select control
public class SelectionControl : MonoBehaviour
{
    [SerializeField]
    private KeyCode up, down, left, right, ready;
    [SerializeField]
    private GameObject nameTag;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private TextMeshProUGUI readyImg;

    private int ID;
    private bool isReady;
    private bool isMoving;
    private int selectedIndex;
    private Image playerImage;
    private TextMeshProUGUI playerName;
    private List<CharacterOption> options;
    private CharacterOption previous;
    private CharacterOption current;
    private float timeToSelect = .2f;


    void Start()
    {
        isMoving = false;
        isReady = false;
        playerName = nameTag.GetComponent<TextMeshProUGUI>();
        playerImage = player.GetComponent<Image>();
        // select a random character
        selectedIndex = Random.Range(0, options.Count);
        previous = options[selectedIndex];
        current = previous;
        current.Select(ID);
        playerImage.sprite = current.GetIdle();
        playerName.SetText(current.GetName());
        StartCoroutine(SelectFrame(current));
        playerImage.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isReady)
        {
            if (Input.GetKeyUp(left) && selectedIndex >= 1 && !isMoving)
            {
                selectedIndex -= 1;
                ChangeCharacter(options[selectedIndex]);
            }
            else if (Input.GetKeyUp(right) && selectedIndex <= options.Count - 2 && !isMoving)
            {
                selectedIndex += 1;
                ChangeCharacter(options[selectedIndex]);
            }
            else if (Input.GetKeyUp(up) && selectedIndex >= 4 && !isMoving)
            {
                selectedIndex -= 4;
                ChangeCharacter(options[selectedIndex]);
            }
            else if (Input.GetKeyUp(down) && selectedIndex <= options.Count - 5 && !isMoving)
            {
                selectedIndex += 4;
                ChangeCharacter(options[selectedIndex]);
            }
        }
        if (Input.GetKeyUp(ready))
        {
            ToggleReady();
        }

    }

    private void ToggleReady()
    {
        isReady = !isReady;
        if (isReady)
        {
            readyImg.transform.RotateAround(transform.position, transform.up, 90f);
        }
        else
        {
            readyImg.transform.RotateAround(transform.position, transform.up, -90f);
        }
    }

    private void ChangeCharacter(CharacterOption opt)
    {
        previous = current;
        current = opt;
        previous.Deselect(ID);
        current.Select(ID);
        playerImage.sprite = opt.GetIdle();
        playerName.SetText(opt.GetName());
        StartCoroutine(SelectFrame(opt));

    }

    private IEnumerator SelectFrame(CharacterOption curr)
    {
        isMoving = true;
        float timeElapsed = 0f;

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);

        while (timeElapsed < timeToSelect)
        {
            if(previous.GetNumberOfSelects() == 0)
            {
                previous.transform.localScale = Vector3.Lerp(targetScale, originalScale, (timeElapsed / timeToSelect));
            }
            curr.transform.localScale = Vector3.Lerp(originalScale, targetScale, (timeElapsed / timeToSelect));

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        isMoving = false;
    }

    public void SetOptions(List<CharacterOption> optionList)
    {
        options = optionList;
    }

    public void SetId(int id)
    {
        ID = id;
    }

}
