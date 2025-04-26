using UnityEngine;

[CreateAssetMenu(menuName = "Game/CoinData")]
public class CoinData : ScriptableObject
{
    public GameObject coinPrefab;
    public int coinValue;

    [Header("Player Coins")]
    public int playerCoins;
}
