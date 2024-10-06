using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    
    public void SwitchScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
