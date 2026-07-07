using System.Collections;
using UnityEngine;

/// <summary>
/// Requisito 4: plataforma que se habilita al responder correctamente.
/// Empieza como "fantasma" (semitransparente y sin colision) y se
/// materializa con un fundido al activarse.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ActivatablePlatform : MonoBehaviour
{
    [SerializeField] private float ghostAlpha = 0.25f;
    [SerializeField] private float fadeDuration = 0.5f;

    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        SetAlpha(ghostAlpha);
        col.enabled = false;   // no se puede pisar hasta acertar
    }

    public void Activate() => StartCoroutine(FadeIn());

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(ghostAlpha, 1f, t / fadeDuration));
            yield return null;
        }
        SetAlpha(1f);
        col.enabled = true;    // ahora si es solida
    }

    private void SetAlpha(float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}