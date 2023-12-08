using UnityEngine;

public class UIEvents2D : MonoBehaviour
{
    public void OnClickBack()
    {
        GameStateManager2D.Instance.QuitToTitle();
    }

    public void OnClickFight()
    {
        GameStateManager2D.Instance.NewGame();
    }

    public void OnClickTutorial()
    {
        GameStateManager2D.Instance.LoadLevel("TutorialScene");
    }

    public void OnClickQuit()
    {
        GameStateManager2D.Instance.CloseApp();
    }
}
