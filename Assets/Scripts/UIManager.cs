using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;

    public void StartGame()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
