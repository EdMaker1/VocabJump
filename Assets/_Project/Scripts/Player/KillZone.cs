using UnityEngine;

/// <summary>
/// Zona invisible muy por debajo del nivel. Si el jugador cae aqui
/// (por ejemplo, tras un salto fallido a un precipicio), pierde una
/// vida y es reposicionado en el ultimo punto seguro, o se recarga
/// el nivel si no hay vidas restantes.
/// </summary>
public class KillZone : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.LoseLife();

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        other.transform.position = respawnPoint.position;
    }
}