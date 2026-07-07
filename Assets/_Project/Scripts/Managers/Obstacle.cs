using System.Collections;
using UnityEngine;

/// <summary>
/// Requisito 7: resta una vida al jugador al contacto.
/// Tras el golpe, empuja al jugador y da invulnerabilidad breve
/// para no perder varias vidas en un mismo toque.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float invulnerabilityTime = 1f;

    private static bool playerInvulnerable;

    private void OnCollisionEnter2D(Collision2D collision) => TryDamage(collision.collider);
    private void OnTriggerEnter2D(Collider2D other) => TryDamage(other);

    private void TryDamage(Collider2D other)
    {
        if (playerInvulnerable || !other.CompareTag("Player")) return;

        GameManager.Instance.LoseLife();

        // Empujon de retroceso
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            rb.velocity = Vector2.zero;
            rb.AddForce((direction + Vector2.up * 0.5f) * knockbackForce, ForceMode2D.Impulse);
        }

        StartCoroutine(InvulnerabilityWindow());
    }

    private IEnumerator InvulnerabilityWindow()
    {
        playerInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        playerInvulnerable = false;
    }
}