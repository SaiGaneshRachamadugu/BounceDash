using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public CoinData coinData;
    public ShopItem[] shopItems;
    public GameObject shopItemPrefab;
    public Transform shopItemParent;
    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI msgText;

    private void Start()
    {
        UpdateCoinUI();
        PopulateShop();
    }

    // Instantiates shop items dynamically from the ShopItem list
    private void PopulateShop()
    {
        foreach (ShopItem item in shopItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopItemParent);
            ShopItemUI itemUI = obj.GetComponent<ShopItemUI>();
            itemUI.Setup(item, this);
        }
    }

    // Attempts to purchase the given item
    public void TryPurchase(ShopItem item)
    {
        if (coinData.playerCoins >= item.price)
        {
            coinData.playerCoins -= item.price;
            UpdateCoinUI();
            Debug.Log("Purchased: " + item.itemName);

            SaveSelectedSkin(item);

            // Save ownership and update UI
            PlayerPrefs.SetInt(item.itemName + "_Owned", 1);
            PlayerPrefs.Save();

            // Update shop UI to show item as owned
            foreach (ShopItemUI ui in shopItemParent.GetComponentsInChildren<ShopItemUI>())
            {
                if (ui.name == item.itemName || ui.name.Contains(item.itemName))
                {
                    ui.MarkAsOwned();
                }
            }
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    // Saves the selected skin into PlayerPrefs and shows a temporary message
    private void SaveSelectedSkin(ShopItem item)
    {
        PlayerPrefs.SetString("SelectedSkinName", item.itemName);
        PlayerPrefs.Save();

        Debug.Log("Skin Purchased: " + item.itemName);

        msgText.gameObject.SetActive(true);
        msgText.text = "Skin Purchased: " + item.itemName;

        // Start coroutine to hide the message after a delay
        StartCoroutine(HidePurchaseMessage());
    }

    // Coroutine to hide the purchase success message after 2 seconds
    private System.Collections.IEnumerator HidePurchaseMessage()
    {
        yield return new WaitForSeconds(2f);
        msgText.gameObject.SetActive(false);
    }
    // Updates the player's coin count displayed in UI
    private void UpdateCoinUI()
    {
        playerCoinsText.text = "Coins: " + coinData.playerCoins.ToString();
    }
}
