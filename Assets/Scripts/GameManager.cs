using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverPanel;
   // public GameObject playerdies;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void GameOver()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
        gameOverPanel.SetActive(true);
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }
}
