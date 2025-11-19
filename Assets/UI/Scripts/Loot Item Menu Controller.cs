using UnityEngine;

public class LootItemMenuController : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private GameObject itemParent;
    private ItemBehavior loot;

    private void Awake()
    {
        loot = itemParent.GetComponent<ItemBehavior>();
        inventory = FindFirstObjectByType<Inventory>();
    }

    public void AddItemToInventory()
    {
        ItemInventoryData data = loot.GetItemInventoryData();
        Item item = ScriptableObject.CreateInstance<Item>();
        item.SetItem(data.ItemRarity, data.ItemIcon, data.Perks, data.PerksDeltas, data.ItemName);
        inventory.AddToInventory(item);
        item.ApplyPerks();
        Destroy(itemParent);
    }
}