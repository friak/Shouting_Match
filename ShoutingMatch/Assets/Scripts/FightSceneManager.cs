using UnityEngine;

public class FightSceneManager : MonoBehaviour
{
    private PlayerController[] players;
    [SerializeField]
    private GameObject playerUIHolder;
    [SerializeField]
    private GameObject playerUIPrefab;
    
    void Start()
    {
        players = FindObjectsOfType<PlayerController>(true);
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
            }
            // Set player character
            players[i].SetCharacter(chSO.m_idle);
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
