using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [SerializeField]
    private CharacterSO defaultCharacter1, defaultCharacter2;

    public CharacterSO[] Players { get; set; }

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
        Players = new CharacterSO[2];
        // default characters for testing purposes
        Players[0] = defaultCharacter1;
        Players[1] = defaultCharacter2;
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
        Players[index] = player;
    }
}
