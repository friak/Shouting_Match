using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO.Ports;

public enum GameState
{
    TITLE,
    PLAY,
    PAUSE,
    GAMEOVER
}
public class GameStateManager : MonoBehaviour
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
    // for testing purposes - we don't have to start at the select scene
    [SerializeField]
    private CharacterScriptableAsset m_player1;
    [SerializeField]
    private CharacterScriptableAsset m_player2;
    [SerializeField]
    private string port1 = "COM3";
    [SerializeField]
    private string port2 = "COM4";
    [SerializeField]
    private int baudRate = 9600;
    public SerialPort SPort1 { get; private set; } 
    public SerialPort SPort2 { get; private set; } 

    public CharacterScriptableAsset Player1 { get { return m_player1; } set { } }
    public CharacterScriptableAsset Player2 { get { return m_player2; } set { } }

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
        SPort1 = new SerialPort(port1, baudRate, Parity.None, 8, StopBits.One);
        SPort2 = new SerialPort(port2, baudRate, Parity.None, 8, StopBits.One);
        // ARDUINO INPUT
        SPort1.ReadTimeout = 1; //Shortest possible read time out.
        SPort1.WriteTimeout = 1; //Shortest possible write time out.
        SPort2.ReadTimeout = 1; 
        SPort2.WriteTimeout = 1;
        OpenPort(SPort1);
        OpenPort(SPort2);
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
        Debug.Log("Quitting game, closing ports ...");
        SPort1.Close();
        SPort2.Close();
        // quit otherwise
        Application.Quit();
    }

    public void LoadLevel(string name)
    {
        if (State == GameState.PAUSE)
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

    public void SetPlayer(int id, CharacterScriptableAsset playerAssets)
    {
        if (id == 1) { Player1 = playerAssets; }
        if (id == 2) { Player2 = playerAssets; }
    }

    private void OpenPort(SerialPort sPort)
    {
        try
        {
            sPort.Open();
            Debug.Log("Port open: " + sPort.PortName);
        }
        catch (System.Exception e)
        {
            Debug.Log("Cannot open port: " + e);
        }
    }
}
