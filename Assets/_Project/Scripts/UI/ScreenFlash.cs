using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Penalizacion visual: destello rojo en pantalla al recibir daño
/// o fallar una pregunta. Se suscribe al Observer (OnPlayerDamaged y OnWrongAnswer).
/// </summary>
[RequireComponent(typeof(Image))]
public class ScreenFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = new Color(0.8f, 0f, 0f, 0.45f);
    [SerializeField] private float flashDuration = 0.4f;

    private Image img;
    private Coroutine current;

    private void Awake()
    {
        img = GetComponent<Image>();
        img.color = Color.clear;
        img.raycastTarget = false;   // que no bloquee los clicks de los botones
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDamaged += Flash;
        GameEvents.OnWrongAnswer += FlashFromWord;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDamaged -= Flash;
        GameEvents.OnWrongAnswer -= FlashFromWord;
    }

    private void FlashFromWord(WordData _) => Flash();

    private void Flash()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        img.color = flashColor;
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;   // funciona aunque el juego este pausado
            img.color = Color.Lerp(flashColor, Color.clear, t / flashDuration);
            yield return null;
        }
        img.color = Color.clear;
    }
}