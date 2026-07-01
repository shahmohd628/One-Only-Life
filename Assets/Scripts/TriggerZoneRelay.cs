using UnityEngine;

public class TriggerZoneRelay : MonoBehaviour
{
    public SpikeController spikeController;

    void Start()
    {
        spikeController = GetComponentInParent<SpikeController>();

        if (spikeController == null)
            Debug.LogWarning("[TriggerZoneRelay] No SpikeController found on parent! Check your hierarchy.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spikeController?.OnPlayerEnteredZone();
        }
    }
}