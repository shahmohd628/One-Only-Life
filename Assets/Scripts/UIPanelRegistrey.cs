using UnityEngine;

public class UIPanelRegistry : MonoBehaviour
{
    [Header("Drag panels here — they can be active or inactive")]
    public GameObject gameOverPanel;
    public GameObject congratsPanel;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[UIPanelRegistry] GameManager not found! Make sure GameManager exists in Level1 and uses DontDestroyOnLoad.");
            return;
        }

        GameManager.Instance.RegisterPanels(gameOverPanel, congratsPanel);
    }
}