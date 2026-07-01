using UnityEngine;

public class SpikeController : MonoBehaviour
{

    [Header("Target")]
    public Transform targetPoint;

    [Header("Timing & Speed")]
    public float delayBeforeMove = 0.5f;
    public float moveSpeed = 4f;

    [Header("Visibility")]
    public bool startHidden = true;


    SpriteRenderer spriteRenderer;
    Collider2D spikeCollider;
    bool triggered = false;
    bool isMoving = false;
    bool reachedTarget = false;
    float delayTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spikeCollider = GetComponent<Collider2D>();

        if (startHidden)
        {
            SetVisible(false);
        }
    }

    void Update()
    {
        if (!triggered || reachedTarget) return; 

        if (!isMoving)
        {
            
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
            {
                isMoving = true;
            }
        }
        else
        {
            MoveSpikeToTarget();
        }
    }

    void MoveSpikeToTarget()
    {
        if (targetPoint == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            transform.position = targetPoint.position;
            reachedTarget = true;
            isMoving = false;
        }
    }

    public void OnPlayerEnteredZone()
    {
        if (triggered) return; 

        triggered = true;
        delayTimer = delayBeforeMove;

        SetVisible(true);
    }

    void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;

        if (spikeCollider != null)
            spikeCollider.enabled = visible;
    }
}