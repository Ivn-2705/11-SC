using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class controller : MonoBehaviour
{
    private const string STRING_VELOCIDAD = "Velocidad";
    private const string STRING_EN_SUELO = "EnSuelo";

    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Salto")]
    public float jumpForce = 16f;
    public float fallMultiplier = 9f;

    [Header("Coyote / Buffer")]
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.12f;

    [Header("Wall Jump")]
    public float wallJumpForceX = 9f;
    public float wallJumpForceY = 16f;
    public float wallSlideSpeed = 2.5f;
    public float wallCheckDistance = 0.6f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    // INPUT
    private float moveInput;
    private bool jumpReleased;

    // ESTADO
    private bool isGrounded;
    private bool isTouchingWall;
    private int wallDirection;

    // TIMERS
    private float coyoteCounter;
    private float jumpBufferCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        CheckGround();  // ✅ Esto evita que empiece en salto
    }

    void Update()
    {
        // --- INPUT CON WAD ---
        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed) moveInput = -1f; // A para izquierda
        if (Keyboard.current.dKey.isPressed) moveInput = 1f;  // D para derecha

        var jumpKey = Keyboard.current.wKey; // W para saltar
        if (jumpKey.wasPressedThisFrame) jumpBufferCounter = jumpBufferTime;
        jumpReleased = jumpKey.wasReleasedThisFrame;

        jumpBufferCounter -= Time.deltaTime;

<<<<<<< Updated upstream
        // ANIMACIONES
=======
>>>>>>> Stashed changes
        ControlarAnimaciones();
    }

    void FixedUpdate()
    {
        DetectarPared();
        Movimiento();
        ManejarTimers();
        SaltoFisico();
        WallSlide();
        AplicarGravedadExtra();
    }

    void Movimiento()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput < 0) sr.flipX = true;
        else if (moveInput > 0) sr.flipX = false;
    }

    void ManejarTimers()
    {
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.fixedDeltaTime;
    }

    void DetectarPared()
    {
        int groundLayer = LayerMask.GetMask("Ground");

        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);

        if (left)
        {
            isTouchingWall = true;
            wallDirection = 1; // Salta hacia la derecha
        }
        else if (right)
        {
            isTouchingWall = true;
            wallDirection = -1; // Salta hacia la izquierda
        }
        else
        {
            isTouchingWall = false;
        }
    }

    void SaltoFisico()
    {
        // SALTO NORMAL + COYOTE + BUFFER
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
            isGrounded = false;
            return;
        }

        // WALL JUMP
        if (jumpBufferCounter > 0f && isTouchingWall && !isGrounded)
        {
            rb.linearVelocity = new Vector2(
                wallDirection * wallJumpForceX,
                wallJumpForceY
            );
            jumpBufferCounter = 0f;
            return;
        }

        // CORTE DE SALTO (Salto variable)
        if (jumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.6f
            );
        }
    }

    void WallSlide()
    {
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                -wallSlideSpeed
            );
        }
    }

    void AplicarGravedadExtra()
    {
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                                 (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void ControlarAnimaciones()
    {
        if (animator != null)
        {
<<<<<<< Updated upstream
            float velocidad = Mathf.Abs(rb.linearVelocity.x);

            // Suavizado 0.05
            animator.SetFloat(STRING_VELOCIDAD, velocidad, 0.05f, Time.deltaTime);

            // En suelo SIN suavizado (cambia al instante)
=======
            animator.SetFloat(STRING_VELOCIDAD, Mathf.Abs(rb.linearVelocity.x), 0.05f, Time.deltaTime);
>>>>>>> Stashed changes
            animator.SetBool(STRING_EN_SUELO, isGrounded);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // ======= NUEVO MÉTODO =======
    void CheckGround()
    {
        int groundLayer = LayerMask.GetMask("Ground");
        Vector2 position = transform.position;
        Vector2 size = bc.size;
        float extraHeight = 0.1f;

        RaycastHit2D hit = Physics2D.BoxCast(position, size, 0f, Vector2.down, extraHeight, groundLayer);
        isGrounded = hit.collider != null;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}

