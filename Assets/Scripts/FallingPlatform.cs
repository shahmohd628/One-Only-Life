using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // ---------- Inspector Fields ----------
    [Header("Falling Settings")]
    [Tooltip("Seconds before the platform falls after player steps on it")]
    public float delayBeforeFall = 1f;
    [Tooltip("Speed at which platform falls")]
    public float fallSpeed = 3f;
    [Tooltip("Empty GameObject at the destination (usually off-screen below)")]
    public Transform targetPoint;

    [Header("Shake Effect (visual warning before fall)")]
    [Tooltip("How much to shake the platform left-right")]
    public float shakeAmount = 0.05f;

    bool playerIsOn = false;
    bool isFalling = false;
    float delayTimer = 0f;
    Vector3 originalPosition;

    // Shake state
    float shakeTimer = 0f;
    bool isShaking = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isFalling)
        {
            FallToTarget();
            return;
        }

        if (playerIsOn)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0.5f && !isShaking)
            {
                isShaking = true;
            }

            if (isShaking)
            {
                ApplyShake();
            }

            if (delayTimer <= 0f)
            {
                StartFalling();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            if (collision.transform.position.y > transform.position.y)
            {
                playerIsOn = true;
                delayTimer = delayBeforeFall;
                Debug.Log("[FallingPlatform] Player stepped on platform. Falling in " + delayBeforeFall + "s");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isFalling)
            {
                playerIsOn = false;
                isShaking = false;
                delayTimer = 0f;
                transform.position = originalPosition;
            }
        }
    }

    void StartFalling()
    {
        isFalling = true;
        isShaking = false;
        transform.position = originalPosition;
    }

    void FallToTarget()
    {
        if (targetPoint == null)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            fallSpeed * Time.deltaTime
        );
    }

    void ApplyShake()
    {
        shakeTimer += Time.deltaTime * 30f; 
        float xOffset = Mathf.Sin(shakeTimer) * shakeAmount;
        transform.position = new Vector3(originalPosition.x + xOffset, transform.position.y, transform.position.z);
    }
}