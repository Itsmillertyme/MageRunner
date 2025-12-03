using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootItemMenuController : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI title;
    [SerializeField] Image icon;
    [SerializeField] Image decoration1;
    [SerializeField] Image decoration2;
    [SerializeField] TMPro.TextMeshProUGUI statsText;
    [Tooltip("The topmost item gameobject in the scene")]
    [SerializeField] private GameObject menu;
    [SerializeField] private Colors colorManager;
    private GameObject item; // THE ITEM CURRENTLY POSSESSING THE UI MENU
    private Inventory inventory;
    private Item activeMenuItem;
    private bool isMenuActive = false;

    public bool IsMenuActive => isMenuActive;

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory>();
    }

    public void ToggleMenuActive() // CLOSE BUTTON INVOKES THIS
    {
        isMenuActive = !isMenuActive; // FLIP STATUS
        menu.SetActive(isMenuActive); // SHOW/HIDE GAMEOBJECT
    }

    public void SetActiveMenuItem(GameObject item)
    {
        ToggleMenuActive();
        this.item = item;
        activeMenuItem = item.GetComponent<ItemBehavior>().GetItem;
        UpdateMenuElements();
    }

    private void UpdateMenuElements() // MATCH ITEM ATTRIBUTES
    {
        title.text = activeMenuItem.ItemName;
        icon.sprite = activeMenuItem.ItemIcon;
        decoration1.color = colorManager.GetItemColor(activeMenuItem.Rarity);
        decoration2.color = decoration1.color;
        statsText.text = PerksToString(activeMenuItem.Perks);
    }

    private string PerksToString(ItemPerk[] array)
    {
        string textContents = "";
        foreach (ItemPerk itemPerk in array)
        {
            switch (itemPerk)
            {
                case PerkDamageIncreaseSpecificSpell perk:
                    textContents += $"{perk.ItemName}: {perk.GetPerkDeltaRounded()}%\n";
                    break;
                case PerkDamageResistance perk:
                    textContents += $"{perk.ItemName}: {perk.GetPerkDeltaRounded()}%\n";
                    break;
                case PerkHealthBoost perk:
                    textContents += $"{perk.ItemName}: {perk.GetPerkDeltaRounded()}%\n";
                    break;
            }
        }
        return textContents;
    }

    public void AddItemToInventory() // INVOKED BY LOOT DROP MENU BUTTON
    {
        Item inventoryItem = ScriptableObject.CreateInstance<Item>();
        inventoryItem.SetItem(activeMenuItem);
        inventory.AddToInventory(inventoryItem);
        inventoryItem.ApplyPerks();
        ToggleMenuActive();
        Destroy(item);
    }
}