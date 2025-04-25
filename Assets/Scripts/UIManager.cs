using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;

    public void StartGame()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
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
