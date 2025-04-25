using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;

    private float score;
    private int coins;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        score += Time.deltaTime;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void AddCoin()
    {
        coins++;
        coinText.text = "Coins: " + coins;
    }
}
