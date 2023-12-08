using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Character select list
public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterSO> characters;
    [SerializeField]
    private GameObject optionsHolder;
    [SerializeField]
    private GameObject optionPrefab;

    private SelectionControl[] players;

    void Start()
    {
        List<OptionButton> optionList = new List<OptionButton>();
        // init character select 
        // TO DO: make the grid flexible (now it's only good up to 8 characters)
        for (int i = 0; i < characters.Count; i++)
        {
            GameObject option = Instantiate(optionPrefab, optionsHolder.transform);
            OptionButton characterOption = option.GetComponent<OptionButton>();
            characterOption.SetCharacter(characters[i], i);
            optionList.Add(characterOption);
            // Debug.Log("pos" + option.transform.position);
        }

        // init players
        players = FindObjectsOfType<SelectionControl>(true);
        for (int i = 0; i < players.Length; i++)
        {
            // call SelectionControl's Start method after creating the options list
            players[i].SetId(i);
            players[i].SetOptions(optionList);
            players[i].gameObject.SetActive(true);
        }
    }

    // check if players are ready
    private void Update()
    {
        bool allPlayersReady = true;
        foreach(SelectionControl player in players)
        {
            allPlayersReady &= player.IsReady;
        }
        if (allPlayersReady)
        {
            GameStateManager2D.Instance.LoadLevel("FightScene");
        }
    }

}
