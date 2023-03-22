using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GAMESTATE
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

    public static GAMESTATE State { get; private set; }

    [SerializeField]
    private string m_TitleScene;
    [SerializeField]
    private string m_TutorialScene;
    [SerializeField]
    private string m_SelectScene;

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


    public void TogglePause()
    {
        if (State == GAMESTATE.PAUSE)
        {
            //Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
            State = GAMESTATE.PLAY;
        }
        else if (State == GAMESTATE.PLAY)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 0f;
            State = GAMESTATE.PAUSE;
        }
    }

    public void NewGame()
    {
        State = GAMESTATE.PLAY;
        _instance.LoadLevel(_instance.m_SelectScene);
    }

    public void QuitToTitle()
    {
        State = GAMESTATE.TITLE;
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
        if(State == GAMESTATE.PAUSE)
        {
            Time.timeScale = 1f;
            State = GAMESTATE.PLAY;
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
}
