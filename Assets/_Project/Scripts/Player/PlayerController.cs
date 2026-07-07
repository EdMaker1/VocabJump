using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla el movimiento horizontal y el salto del jugador.
/// Requisito 1: velocidad configurable desde el Inspector.
/// Requisito 2: deteccion de suelo mediante Physics2D.OverlapCircle.
/// Usa el nuevo Input System (paquete Input System).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;        // Requisito 1: configurable
    [SerializeField] private float jumpForce = 12f;

    [Header("Deteccion de Suelo (Requisito 2)")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Feel del salto")]
    [SerializeField] private float coyoteTime = 0.1f;     // margen tras salir de plataforma
    [SerializeField] private float jumpBufferTime = 0.1f; // margen si pulsa antes de aterrizar

    private Rigidbody2D rb;
    private PlayerControls controls;
    private float moveInput;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool facingRight = true;

    // Propiedad publica para que el Animator u otros sistemas la consulten
    public bool IsGrounded => isGrounded;
    public float HorizontalSpeed => Mathf.Abs(rb.velocity.x);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();

        // Suscripcion a las acciones del Input System
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        controls.Gameplay.Move.canceled += ctx => moveInput = 0f;
        controls.Gameplay.Jump.performed += ctx => jumpBufferCounter = jumpBufferTime;
    }
    private void OnEnable()
    {
        controls ??= new PlayerControls();   // si aun no existe, la crea
        controls.Gameplay.Enable();
    }

    private void OnDisable() => controls?.Gameplay.Disable();   // solo si existe

    private void Update()
    {
        // Requisito 2: deteccion de suelo con OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Coyote time: pequeño margen para saltar tras dejar el borde
        coyoteCounter = isGrounded ? coyoteTime : coyoteCounter - Time.deltaTime;

        // Jump buffer: registra el salto aunque se pulse un instante antes de aterrizar
        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        HandleFlip();
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal en FixedUpdate (fisicas)
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // reset vertical para salto consistente
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleFlip()
    {
        if ((moveInput > 0f && !facingRight) || (moveInput < 0f && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    // Visualiza el radio de deteccion en el editor (solo desarrollo)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
