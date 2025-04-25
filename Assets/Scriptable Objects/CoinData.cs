using UnityEngine;

[CreateAssetMenu(menuName = "Game/CoinConfig")]
public class CoinData : ScriptableObject
{
    public GameObject coinPrefab;
    public int coinValue;
}
