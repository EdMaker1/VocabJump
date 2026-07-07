using UnityEngine;

/// <summary>
/// Zona de activacion del nivel (Requisito 3). Al entrar el jugador,
/// lanza una pregunta. Si acierta: habilita plataformas (Requisito 4)
/// y/o elimina obstaculos.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class QuestionZone : MonoBehaviour
{
    [Header("Pregunta de esta zona")]
    [SerializeField] private WordData word;

    [Header("Que ocurre al acertar")]
    [SerializeField] private ActivatablePlatform[] platformsToActivate;
    [SerializeField] private GameObject[] obstaclesToRemove;

    private bool answered;

    // Al agregar el componente, el collider se marca como trigger automaticamente
    private void Reset() => GetComponent<BoxCollider2D>().isTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (answered || !other.CompareTag("Player")) return;
        QuestionManager.Instance.ShowQuestion(word, OnCorrectAnswer);
    }

    private void OnCorrectAnswer()
    {
        answered = true;

        foreach (ActivatablePlatform p in platformsToActivate)
            if (p != null) p.Activate();

        foreach (GameObject o in obstaclesToRemove)
            if (o != null) o.SetActive(false);

        GetComponent<BoxCollider2D>().enabled = false;  // la zona ya no se repite
    }
}