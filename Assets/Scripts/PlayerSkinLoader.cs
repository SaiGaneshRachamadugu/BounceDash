using UnityEngine;

public class PlayerSkinLoader : MonoBehaviour
{
    public ShopItem[] availableSkins;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        LoadSkin();
    }

    private void LoadSkin()
    {
        string savedSkinName = PlayerPrefs.GetString("SelectedSkinName", "");

        if (!string.IsNullOrEmpty(savedSkinName))
        {
            foreach (var skin in availableSkins)
            {
                if (skin.itemName == savedSkinName)
                {
                    spriteRenderer.sprite = skin.itemIcon;
                    Debug.Log("Loaded Skin: " + savedSkinName);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("No saved skin found, using default.");
        }
    }
}
