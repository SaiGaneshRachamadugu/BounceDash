using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI priceText;
    private ShopItem shopItem;
    private ShopManager shopManager;

    public void Setup(ShopItem item, ShopManager manager)
    {
        shopItem = item;
        shopManager = manager;
        iconImage.sprite = item.itemIcon;
        priceText.text = item.price.ToString() + " Coins";
    }

    public void OnBuyButtonClick()
    {
        shopManager.TryPurchase(shopItem);
    }
}
