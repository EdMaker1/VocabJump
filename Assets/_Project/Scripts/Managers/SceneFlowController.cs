using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Navegacion entre escenas. Sus metodos publicos se conectan
/// directamente a los eventos OnClick de los botones de UI.
/// </summary>
public class SceneFlowController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;   // por si venimos de una pausa
        SceneManager.LoadScene(sceneName);
    }

    public void StartNewRun()
    {
        SessionData.ResetRun();
        LoadScene("Login");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // para probar en el editor
#endif
    }
}