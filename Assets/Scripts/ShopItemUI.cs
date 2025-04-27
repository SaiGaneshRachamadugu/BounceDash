using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI priceText;
    public GameObject tickMark;
    private ShopItem shopItem;
    private ShopManager shopManager;
    private bool isOwned = false;

    public void Setup(ShopItem item, ShopManager manager)
    {
        shopItem = item;
        shopManager = manager;
        iconImage.sprite = item.itemIcon;
        priceText.text = item.price.ToString() + " Coins";

        CheckOwnership();
    }

    private void CheckOwnership()
    {
        if (PlayerPrefs.GetInt(shopItem.itemName + "_Owned", 0) == 1)
        {
            isOwned = true;
            tickMark.SetActive(true);
            priceText.text = "Owned";
        }
        else
        {
            isOwned = false;
            tickMark.SetActive(false);
        }
    }

    public void OnBuyButtonClick()
    {
        if (!isOwned)
        {
            shopManager.TryPurchase(shopItem);
        }
        else
        {
            Debug.Log("Already owned: " + shopItem.itemName);
        }
    }

    public void MarkAsOwned()
    {
        isOwned = true;
        tickMark.SetActive(true);
        priceText.text = "Owned";
    }
}
