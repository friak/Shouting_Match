using UnityEngine;

public class FightSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PLayer1;
    [SerializeField]
    private GameObject PLayer2;
    [SerializeField]
    private GameObject Player1UI;
    [SerializeField]
    private GameObject Player2UI;

    private GameObject player1Prefab;
    private GameObject player2Prefab;

    // Start is called before the first frame update
    void Awake()
    {
        CharacterScriptableAsset asset1 = GameStateManager.Instance.Player1;
        CharacterScriptableAsset asset2 = GameStateManager.Instance.Player2;
        // instantiate
         player1Prefab = Instantiate(asset1.playerPrefab, PLayer1.transform);
         player2Prefab = Instantiate(asset2.playerPrefab, PLayer2.transform);
        // set the controller 

        player1Prefab.GetComponent<PlayerController>().SetupController(true, player2Prefab.transform);
        player2Prefab.GetComponent<PlayerController>().SetupController(false, player1Prefab.transform);

        // set player info
         PLayer1.GetComponent<Player>().InitPLayer(asset1);
         PLayer2.GetComponent<Player>().InitPLayer(asset2);
         // set player UI
         Player1UI.GetComponent<PlayerFightingUI>().SetPlayerUI(asset1);
         Player2UI.GetComponent<PlayerFightingUI>().SetPlayerUI(asset2);
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
