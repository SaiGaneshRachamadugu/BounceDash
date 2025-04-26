using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int price;
}
