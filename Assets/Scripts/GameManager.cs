using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    public int totalLevels = 5;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gameOverSFX;
    public AudioClip levelCompleteSFX;
    public AudioClip clickSFX;

    GameObject gameOverPanel;
    GameObject congratsPanel;
    GameObject pausePanel;

    int deathCount = 0;
    int currentLevel = 1;
    bool isDead = false;
    bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Don't pause if Game Over or Congrats panel is open
            if ((gameOverPanel != null && gameOverPanel.activeSelf) ||
                (congratsPanel != null && congratsPanel.activeSelf))
                return;

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        isDead = false;
        isPaused = false;

        gameOverPanel = null;
        congratsPanel = null;
        pausePanel = null;
    }

    public void RegisterPanels(GameObject overPanel, GameObject congrats, GameObject pause)
    {
        gameOverPanel = overPanel;
        congratsPanel = congrats;
        pausePanel = pause;

        SetPanel(gameOverPanel, false);
        SetPanel(congratsPanel, false);
        SetPanel(pausePanel, false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        if (pausePanel == null) return;

        SetPanel(pausePanel, true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel == null) return;

        SetPanel(pausePanel, false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PlayerDied()
    {
        if (isDead) return;

        isDead = true;
        deathCount++;

        PlaySFX(gameOverSFX);
        SetPanel(gameOverPanel, true);
    }

    public void LevelComplete()
    {
        PlaySFX(levelCompleteSFX);

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level" + totalLevels)
        {
            SetPanel(congratsPanel, true);
        }
        else
        {
            int current = int.Parse(sceneName.Replace("Level", ""));
            int nextLevel = current + 1;

            SceneManager.LoadScene("Level" + nextLevel);
        }
    }

    public int GetDeathCount() => deathCount;

    public void PlayClickSFX()
    {
        if (clickSFX != null)
            AudioSource.PlayClipAtPoint(clickSFX, Camera.main.transform.position);
    }

    public void OnRestartButtonClicked()
    {
        PlayClickSFX();

        Time.timeScale = 1f;
        isPaused = false;

        Invoke(nameof(RestartFromLevel1), 0.2f);
    }

    public void OnMenuButtonClicked()
    {
        PlayClickSFX();

        Time.timeScale = 1f;
        isPaused = false;

        Invoke(nameof(LoadMenu), 0.2f);
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void RestartFromLevel1()
    {
        currentLevel = 1;
        SceneManager.LoadScene("Level1");
    }

    void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void SetPanel(GameObject panel, bool active)
    {
        if (panel != null)
            panel.SetActive(active);
        else
            Debug.LogWarning("[GameManager] Panel is null — did you register all panels?");
    }
}