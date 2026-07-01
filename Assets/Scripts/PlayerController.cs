using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float fallGravityMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.5f;

    [Header("Ground Check")]

    public Transform groundCheck;

    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Effects")]

    public GameObject jumpEffectObject;
    public float jumpEffectDuration = 0.3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSFX;

    [Header("Control Flags")]
    public bool isControlledByPlayer = true;
    public bool reverseControls = false;

    Rigidbody2D rb;
    bool isGrounded = false;
    bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (jumpEffectObject != null)
            jumpEffectObject.SetActive(false);
    }

    void Update()
    {
        if (!isControlledByPlayer) return;

        HandleMovement();
        HandleJump();
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyEnhancedGravity();
    }

    void CheckGround()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("[PlayerController] groundCheck is not assigned! Drag your GroundCheck child object into the Inspector.");
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleMovement()
    {
        float input = Input.GetAxis("Horizontal");
        if (reverseControls) input = -input;

        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);

        if (input > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (input < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            ShowJumpEffect();
            PlayJumpSound();
        }
    }

    void ApplyEnhancedGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void ShowJumpEffect()
    {
        if (jumpEffectObject == null) return;
        jumpEffectObject.SetActive(true);
       
        CancelInvoke(nameof(HideJumpEffect));
        Invoke(nameof(HideJumpEffect), jumpEffectDuration);
    }

    void HideJumpEffect()
    {
        if (jumpEffectObject != null)
            jumpEffectObject.SetActive(false);
    }

    void PlayJumpSound()
    {
        if (audioSource != null && jumpSFX != null)
            audioSource.PlayOneShot(jumpSFX);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
            GameManager.Instance.PlayerDied();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            Die();
            GameManager.Instance.PlayerDied();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;


        isControlledByPlayer = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void SetReverseControls(bool reverse) => reverseControls = reverse;
    public void SetControlEnabled(bool enabled) => isControlledByPlayer = enabled;
}