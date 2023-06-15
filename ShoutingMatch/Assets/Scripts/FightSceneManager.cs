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
            Player player = GameStateManager.Instance.Players[i];
            //Create the UI - set profile and name
            GameObject playerGO = Instantiate(playerUIPrefab, playerUIHolder.transform);
            PlayerUI playerUI = playerGO.GetComponent<PlayerUI>();
            playerUI.SetName(player.Character.m_name);
            playerUI.SetProfile(player.Character.m_profile);
            if (i % 2 == 1)
            {
                playerUI.Flip();
                players[i].FlipCharacter();
            }

            // Set player
            players[i].SetPlayer(player);
            players[i].SetHealthBar(playerUI, player.Character.m_health);
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
