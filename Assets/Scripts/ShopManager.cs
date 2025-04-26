using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public CoinData coinData; // Reference to your CoinData ScriptableObject
    public ShopItem[] shopItems;
    public GameObject shopItemPrefab;
    public Transform shopItemParent;

    public TextMeshProUGUI playerCoinsText;

    private void Start()
    {
        UpdateCoinUI();
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (ShopItem item in shopItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopItemParent);
            ShopItemUI itemUI = obj.GetComponent<ShopItemUI>();
            itemUI.Setup(item, this);
        }
    }

    public void TryPurchase(ShopItem item)
    {
        if (coinData.playerCoins >= item.price)
        {
            coinData.playerCoins -= item.price;
            UpdateCoinUI();
            Debug.Log("Purchased: " + item.itemName);

            // TODO: Unlock functionality (equip skin, mark as owned, etc.)
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void UpdateCoinUI()
    {
        playerCoinsText.text = "Coins: " + coinData.playerCoins.ToString();
    }
}
