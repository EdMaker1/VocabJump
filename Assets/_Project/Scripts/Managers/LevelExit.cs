using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Meta del nivel: al llegar el jugador, registra las estadisticas
/// en la sesion y carga la pantalla de resumen (Requisito 10).
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class LevelExit : MonoBehaviour
{
    [Tooltip("Escena que sigue DESPUES del resumen: Level2 o FinalSummary")]
    [SerializeField] private string nextLevelAfterSummary = "Level2";

    [Tooltip("Resumen a cargar: LevelSummary (parcial) o FinalSummary (final)")]
    [SerializeField] private string summaryScene = "LevelSummary";

    private void Reset() => GetComponent<BoxCollider2D>().isTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager gm = GameManager.Instance;
        SessionData.RegisterLevelResults(
            gm.CorrectAnswers,
            gm.TotalAnswers - gm.CorrectAnswers,
            gm.WordsLearned,
            nextLevelAfterSummary
        );

        Time.timeScale = 1f;
        SceneManager.LoadScene(summaryScene);
    }
}