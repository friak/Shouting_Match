using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    TITLE,
    PLAY,
    PAUSE,
    GAMEOVER
}

// Singleton with static initialization: thread safe without explicitly coding for it,
// relies on the common language runtime to initialize the variable
public sealed class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            return _instance;
        }
        private set { }
    }

    public static GameState State { get; private set; }

    [SerializeField]
    private string m_TitleScene;
    [SerializeField]
    private string m_TutorialScene;
    [SerializeField]
    private string m_SelectScene;

    [SerializeField]// for testing purposes - we don't have to start at the select scene
    private CharacterSO defaultCharacter1, defaultCharacter2;

    public Player[] Players { get; set; }



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
        Players = new Player[2];
        // default characters for testing purposes
        Dictionary<string, int> d1 = new Dictionary<string, int>();
        d1.Add("Forward",4);
        d1.Add("Backward",5);
        d1.Add("Block",3);
        d1.Add("Jump",2);
        d1.Add("Attack1",1);
        d1.Add("Attack2", 111);

        Dictionary<string, int> d2 = new Dictionary<string, int>();
        d2.Add("Forward",8);
        d2.Add("Backward",9);
        d2.Add("Block",7);
        d2.Add("Jump",6);
        d2.Add("Attack1",200);
        d2.Add("Attack2",222);
        
        Players[0] = new Player("/dev/cu.usbmodem1422101", defaultCharacter1, d1);
        Players[1] = new Player("/dev/cu.usbmodem141301", defaultCharacter2, d2);
    }

    public void TogglePause()
    {
        if (State == GameState.PAUSE)
        {
            //Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
            State = GameState.PLAY;
        }
        else if (State == GameState.PLAY)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 0f;
            State = GameState.PAUSE;
        }
    }

    public void NewGame()
    {
        State = GameState.PLAY;
        _instance.LoadLevel(_instance.m_SelectScene);
    }

    public void QuitToTitle()
    {
        State = GameState.TITLE;
        _instance.LoadLevel(_instance.m_TitleScene);
    }

    public void CloseApp()
    {
        //Test when running the game in the Editor,
        Debug.Log("Quitting game ...");
        // quit otherwise
        Application.Quit();
    }

    public void LoadLevel(string name)
    {
        if(State == GameState.PAUSE)
        {
            Time.timeScale = 1f;
            State = GameState.PLAY;
        }
        Debug.Log("opening scene: " + name);
        SceneManager.LoadScene(name);
        //StartCoroutine(CoLoadLevel(name));
    }

    IEnumerator CoLoadLevel(string scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void SetPlayer(int index, CharacterSO player)
    {
        if (index > Players.Length) return;
        Players[index].ChangeCharacter(player);
    }
}
