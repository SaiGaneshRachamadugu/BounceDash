using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;

    private float score;
    private int coins;

    [Header("Coin Data Reference")]
    public CoinData coinData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Initialize coin count from saved CoinData
        coins = coinData.playerCoins;
        UpdateCoinUI();
    }

    private void Update()
    {
        // Increase score over time and update UI
        score += Time.deltaTime;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    // Called when player collects a coin
    public void AddCoin()
    {
        coins++;
        coinData.playerCoins = coins;
        UpdateCoinUI();
    }

    // Updates the displayed coin count
    private void UpdateCoinUI()
    {
        coinText.text = "Coins: " + coins;
    }
}
