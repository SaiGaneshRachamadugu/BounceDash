using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance for easy global access
    public static GameManager Instance;

    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Loads the main menu scene (Scene at build index 0)
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    // Handles game over logic
    public void GameOver()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
        gameOverPanel.SetActive(true);
    }

    // Restarts the current game scene
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
