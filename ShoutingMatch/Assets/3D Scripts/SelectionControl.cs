using System.Collections.Generic;
using System.Collections;
using System.IO.Ports;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public enum selectState
{
    SETCHARACTER,
    SETLOUD,
    SETQUIET,
    READY
}

// Character select control
public class SelectionControl : MonoBehaviour
{
    private static int idCount = 0;
    private int playerId;

    private SerialPort sPort;

    [SerializeField]
    private KeyCode up, down, left, right, ready, deselect;
    [SerializeField]
    private GameObject nameTag;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private TextMeshProUGUI readyImg;
    [SerializeField]
    private TextMeshProUGUI instruction;
    [SerializeField]
    private selectState state;

    [SerializeField]
    private bool isPlayer1 = false;

    private int characterID;
    private bool isMoving;
    private int selectedIndex;
    private Image playerImage;
    private TextMeshProUGUI playerName;
    private List<OptionButton> options;
    private OptionButton previous;
    private OptionButton current;
    private float timeToSelect = .2f;



    public bool IsReady { get; private set; }

    void Start()
    {
        playerId = idCount++;
        isMoving = false;
        IsReady = false;
        instruction.gameObject.SetActive(false);
        playerName = nameTag.GetComponent<TextMeshProUGUI>();
        playerImage = player.GetComponent<Image>();

        // select a random character
        selectedIndex = Random.Range(0, options.Count);
        previous = options[selectedIndex];
        current = previous;
        current.Select(characterID);
        playerImage.sprite = current.GetScriptableObject().m_profile;
        playerName.SetText(current.GetScriptableObject().m_name);
        StartCoroutine(CoSelectFrame(current));
        playerImage.gameObject.SetActive(true);

        sPort = isPlayer1 ? GameStateManager.Instance.SPort1 : GameStateManager.Instance.SPort2;
    }

    void Update()
    {
        int data = 0;
        // Arduino Communication
        if (sPort.IsOpen)
        {
            try
            {
                 data = CheckIfDataReceived();
            }
            catch (System.Exception)
            {
            }
        }
        //Keyboard Controls
        if (state == selectState.SETCHARACTER)
        {
            if ((Input.GetKeyUp(left) || data == -1) && selectedIndex >= 1 && !isMoving)
            {
                selectedIndex -= 1;
                ChangeCharacter(options[selectedIndex]);
            }
            else if ((Input.GetKeyUp(right) || data == 1) && selectedIndex <= options.Count - 2 && !isMoving)
            {
                selectedIndex += 1;
                ChangeCharacter(options[selectedIndex]);
            }
            else if ((Input.GetKeyUp(up) || data == 3) && selectedIndex >= 4 && !isMoving)
            {
                selectedIndex -= 4;
                ChangeCharacter(options[selectedIndex]);
            }
            else if ((Input.GetKeyUp(down) || data == 4) && selectedIndex <= options.Count - 5 && !isMoving)
            {
                selectedIndex += 4;
                ChangeCharacter(options[selectedIndex]);
            }
        }
        if ((Input.GetKeyUp(ready) || data >= 100) && state != selectState.READY)
        {
            ChangeSelectState();
        }
        if (Input.GetKeyUp(deselect) && state == selectState.READY)
        {
            ChangeSelectState();
        }

    }

    public int CheckIfDataReceived()
    {
        // sPort.BaseStream.Flush();
        // sPort.DiscardInBuffer();
        sPort.Write("V");       // ask for data
        string input = sPort.ReadLine();
        Debug.Log("ARDUINO: " + input);
        return int.Parse(input);

    }

    private void ChangeSelectState()
    {
        switch (state)
        {
            case selectState.SETCHARACTER:
                {
                    instruction.gameObject.SetActive(true);
                    instruction.text = "Give your loudest shout!";
                    // check if shout was detected ...

                    state = selectState.SETLOUD;
                    return;
                }
            case selectState.SETLOUD:
                {
                    instruction.text = "Now your quietest...";
                    // check if shout was detected ...

                    state = selectState.SETQUIET;
                    return;
                }
            case selectState.SETQUIET:
                {
                    GameStateManager.Instance.SetPlayer(playerId, current.GetScriptableObject());
                    readyImg.transform.RotateAround(transform.position, transform.up, 90f);
                    state = selectState.READY;
                    IsReady = true;
                    return;
                }
            case selectState.READY:
                {
                    instruction.gameObject.SetActive(false);
                    readyImg.transform.RotateAround(transform.position, transform.up, -90f);
                    state = selectState.SETCHARACTER;
                    IsReady = false;
                    return;
                }
        }
    }


    private void ChangeCharacter(OptionButton opt)
    {
        previous = current;
        current = opt;
        previous.Deselect(characterID);
        current.Select(characterID);
        playerImage.sprite = opt.GetScriptableObject().m_profile;
        playerName.SetText(opt.GetScriptableObject().m_name);
        StartCoroutine(CoSelectFrame(opt));

    }

    private IEnumerator CoSelectFrame(OptionButton curr)
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

    public void SetOptions(List<OptionButton> optionList)
    {
        options = optionList;
    }

    public void SetId(int id)
    {
        characterID = id;
    }

    public CharacterScriptableAsset GetSelectedCharacter()
    {
        return current.GetScriptableObject(); }
}
