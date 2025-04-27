using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
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
        
        coins = coinData.playerCoins;
        UpdateCoinUI();
    }

    private void Update()
    {
        score += Time.deltaTime;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void AddCoin()
    {
        coins++;
        coinData.playerCoins = coins;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = "Coins: " + coins;
    }
}
