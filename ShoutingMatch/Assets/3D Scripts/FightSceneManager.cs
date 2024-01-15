using UnityEngine;

public class FightSceneManager : MonoBehaviour
{
    private static FightSceneManager _instance;
    public static FightSceneManager Instance
    {
        get
        {
            return _instance;
        }
        private set { }
    }
    [SerializeField]
    private GameObject PLayer1;
    [SerializeField]
    private GameObject PLayer2;

    private GameObject player1Prefab;
    private GameObject player2Prefab;
    private PlayerFightingUI pl1UI;
    private PlayerFightingUI pl2UI;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
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
    }

}
