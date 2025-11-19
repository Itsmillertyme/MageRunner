using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Player player;

    [SerializeField] private Item[] items;

    public Item[] ItemInventory => items;

    private void Start()
    {
        player = PlayerAbilities.Instance.PlayerSO;
        items = new Item[player.InventoryCapacityMax];
    }

    private void Update()
    {
        RemoveAllPerks();
    }

    public void RemoveAllPerks()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    items[i].RemovePerks();
                    items[i] = null;
                }
            }
        }
    }

    public void AddToInventory(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) // IF SLOT AVAILABLE
            {
                items[i] = item;
                return;
            }
        }
        
        // IF ALL SLOTS FULL, REPLACE AN ITEM RANDOMLY
        ReplaceItemInInventory(item);
    }

    private void ReplaceItemInInventory(Item item)
    {
        // CHOOSE RANDOM SLOT TO REPLACE
        int randomSlot = UtilityTools.RandomVarianceInt(0, items.Length - 1);
        items[randomSlot] = null;

        // THEN ADD TO INVENTORY
        AddToInventory(item);
    }

    public void IncreaseCapacity()
    {
        if (player.InventoryCapacityCurrent == player.InventoryCapacityMax)
        {
            return;
        }
        player.IncreaseInventoryCapacity();
    }
}