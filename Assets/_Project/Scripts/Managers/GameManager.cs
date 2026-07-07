using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton de estado global: vidas (Requisito 7) y palabras aprendidas (Requisito 8).
/// Se suscribe al Observer: respuestas incorrectas restan vida,
/// correctas suman al contador de palabras.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Vidas (Requisito 7)")]
    [SerializeField] private int maxLives = 3;

    public int CurrentLives { get; private set; }
    public int WordsLearned { get; private set; }
    public int TotalAnswers { get; private set; }
    public int CorrectAnswers { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        CurrentLives = maxLives;
    }

    private void OnEnable()
    {
        GameEvents.OnCorrectAnswer += HandleCorrectAnswer;
        GameEvents.OnWrongAnswer += HandleWrongAnswer;
    }

    private void OnDisable()
    {
        GameEvents.OnCorrectAnswer -= HandleCorrectAnswer;
        GameEvents.OnWrongAnswer -= HandleWrongAnswer;
    }

    private void Start()
    {
        // Notifica el estado inicial para que el HUD arranque sincronizado
        GameEvents.RaiseLivesChanged(CurrentLives);
        GameEvents.RaiseWordsLearnedChanged(WordsLearned);
    }

    private void HandleCorrectAnswer(WordData word)
    {
        WordsLearned++;
        CorrectAnswers++;
        TotalAnswers++;
        GameEvents.RaiseWordsLearnedChanged(WordsLearned);
    }

    private void HandleWrongAnswer(WordData word)
    {
        TotalAnswers++;
        LoseLife();   // Requisito 7: acumulacion de respuestas incorrectas
    }

    /// <summary>Requisito 7: tambien la llaman los obstaculos al contacto.</summary>
    public void LoseLife()
    {
        if (CurrentLives <= 0) return;

        CurrentLives--;
        GameEvents.RaiseLivesChanged(CurrentLives);
        GameEvents.RaisePlayerDamaged();   // dispara el flash rojo

        if (CurrentLives <= 0) GameOver();
    }

    private void GameOver()
    {
        GameEvents.RaiseGameOver();
        // Por ahora reinicia el nivel; en la Fase 5 ira a una pantalla de Game Over
        Time.timeScale = 1f;
        Invoke(nameof(ReloadLevel), 1.5f);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}