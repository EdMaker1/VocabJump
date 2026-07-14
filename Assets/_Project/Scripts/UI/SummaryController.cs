using TMPro;
using UnityEngine;

/// <summary>
/// Requisito 10: pantalla de resumen con porcentaje de respuestas
/// correctas y palabras nuevas adquiridas. Sirve para el resumen
/// parcial (por nivel) y el final (acumulado).
/// </summary>
public class SummaryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;

    [Tooltip("Marcar solo en la escena FinalSummary")]
    [SerializeField] private bool isFinalSummary = false;

    private void Start()
    {
        string name = SessionData.PlayerName;

        if (isFinalSummary)
        {
            titleText.text = $"Congratulations, {name}!";
            statsText.text =
                $"Total correct answers: {SessionData.TotalPercentage()}%\n" +
                $"New words learned: {SessionData.TotalWords}";
        }
        else
        {
            titleText.text = $"Level complete, {name}!";
            statsText.text =
                $"Correct answers: {SessionData.LastLevelPercentage()}%\n" +
                $"New words learned: {SessionData.LastLevelWords}";
        }
    }
}