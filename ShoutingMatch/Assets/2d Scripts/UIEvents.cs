using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public void OnClickBack()
    {
        GameStateManager.Instance.QuitToTitle();
    }

    public void OnClickFight()
    {
        GameStateManager.Instance.NewGame();
    }

    public void OnClickTutorial()
    {
        GameStateManager.Instance.LoadLevel("TutorialScene");
    }

    public void OnClickQuit()
    {
        GameStateManager.Instance.CloseApp();
    }
}
