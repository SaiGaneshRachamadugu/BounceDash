using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;

    //Loads the Game scene on button click
    public void StartGame()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        SceneManager.LoadScene(1);
    }

    //Loads the main menu scene (Scene at build index 0)
    private void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);

    }

    //Quits the game application 
    public void QuitGame()
    {
        Application.Quit();
    }
}
