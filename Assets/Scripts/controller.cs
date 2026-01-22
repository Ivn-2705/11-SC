using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class controller : MonoBehaviour
{
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

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // INPUT
    private float moveInput;
    private bool jumpPressed;
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
    }

    void Update()
    {
        // INPUT
        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed) moveInput = -1f;
        if (Keyboard.current.dKey.isPressed) moveInput = 1f;

        var wKey = Keyboard.current.wKey;
        if (wKey.wasPressedThisFrame) jumpBufferCounter = jumpBufferTime;
        jumpReleased = wKey.wasReleasedThisFrame;

        jumpBufferCounter -= Time.deltaTime;
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
            wallDirection = 1;
        }
        else if (right)
        {
            isTouchingWall = true;
            wallDirection = -1;
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

        // CORTE DE SALTO
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
