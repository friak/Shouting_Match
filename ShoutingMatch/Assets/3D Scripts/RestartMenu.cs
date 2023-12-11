using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    public void OnClickTitle()
    {
        Debug.Log("Should go back to title scene.");
    }

    public void OnclickFight()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
