using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application quit");
    }
}
