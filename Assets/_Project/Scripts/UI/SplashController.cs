using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pantalla de presentacion: fade-in del logo, pausa y fade-out,
/// y carga automatica del menu principal.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class SplashController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float holdDuration = 2f;
    [SerializeField] private string nextScene = "MainMenu";

    private IEnumerator Start()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;

        yield return Fade(cg, 0f, 1f);              // fade-in
        yield return new WaitForSeconds(holdDuration);
        yield return Fade(cg, 1f, 0f);              // fade-out

        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator Fade(CanvasGroup cg, float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        cg.alpha = to;
    }
}