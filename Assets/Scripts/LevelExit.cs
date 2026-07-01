using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Optional: Animate door before loading")]
    [Tooltip("If true, a short pause happens before loading next level")]
    public bool delayBeforeLoad = true;
    public float loadDelay = 0.8f;

    bool levelCompleted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;

            if (delayBeforeLoad)
                Invoke(nameof(TriggerLevelComplete), loadDelay);
            else
                TriggerLevelComplete();
        }
    }

    void TriggerLevelComplete()
    {
        GameManager.Instance.LevelComplete();
    }
}