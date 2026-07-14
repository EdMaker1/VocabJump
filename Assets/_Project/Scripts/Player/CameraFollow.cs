using UnityEngine;

/// <summary>
/// Seguimiento suave de camara al jugador, con altura minima
/// para no hundirse bajo el nivel.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;        // el Player
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector2 offset = new Vector2(2f, 1f);
    [SerializeField] private float minY = 0f;         // altura minima de la camara

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            Mathf.Max(target.position.y + offset.y, minY),
            transform.position.z);   // conserva el Z de la camara (-10)

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}