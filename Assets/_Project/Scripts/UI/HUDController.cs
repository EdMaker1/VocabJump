using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Requisito 8: HUD con vidas restantes (corazones), contador de
/// palabras aprendidas y barra de progreso del nivel.
/// Solo escucha eventos (Observer): no conoce al GameManager ni al QuestionManager.
/// </summary>
public class HUDController : MonoBehaviour
{
    [Header("Vidas")]
    [SerializeField] private Image[] heartIcons;          // 3 corazones

    [Header("Palabras aprendidas")]
    [SerializeField] private TextMeshProUGUI wordsText;

    [Header("Progreso del nivel")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private int totalQuestionsInLevel = 3;   // ajustar por nivel

    private void OnEnable()
    {
        GameEvents.OnLivesChanged += UpdateLives;
        GameEvents.OnWordsLearnedChanged += UpdateWords;
    }

    private void OnDisable()
    {
        GameEvents.OnLivesChanged -= UpdateLives;
        GameEvents.OnWordsLearnedChanged -= UpdateWords;
    }

    private void UpdateLives(int lives)
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            // Corazon lleno = opaco; perdido = casi invisible
            Color c = heartIcons[i].color;
            c.a = (i < lives) ? 1f : 0.15f;
            heartIcons[i].color = c;
        }
    }

    private void UpdateWords(int count)
    {
        wordsText.text = $"Words: {count}";
        if (progressBar != null)
            progressBar.value = Mathf.Clamp01((float)count / totalQuestionsInLevel);
    }
}