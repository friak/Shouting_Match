using UnityEngine;

public class FightSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerUIHolder;
    [SerializeField]
    private GameObject playerUIPrefab;
    [SerializeField]
    private PlayerController[] players;

    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            CharacterSO chSO = GameStateManager.Instance.Players[i];
            //Create the UI - set profile and name
            GameObject player = Instantiate(playerUIPrefab, playerUIHolder.transform);
            PlayerUI playerUI = player.GetComponent<PlayerUI>();
            playerUI.SetName(chSO.m_name);
            playerUI.SetProfile(chSO.m_profile);
            if (i % 2 == 1)
            {
                playerUI.Flip();
                players[i].FlipCharacter();
            }

            // Set player character
            players[i].SetCharacter(chSO);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(PlayerController player in players)
        {
            if (player.IsDead)
            {
                Debug.Log("GAME OVER!\n" + player.name + " lost!");
            }
        }
    }
}
