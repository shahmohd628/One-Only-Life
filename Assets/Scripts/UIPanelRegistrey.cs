using UnityEngine;

public class UIPanelRegistry : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject congratsPanel;
    public GameObject pausePanel;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        GameManager.Instance.RegisterPanels(gameOverPanel, congratsPanel, pausePanel);
    }
}