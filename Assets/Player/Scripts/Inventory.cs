using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Player player;

    private Item[] items;

    public Item[] ItemInventory => items;

    private void Awake()
    {
        items = new Item[player.InventoryCapacityMax];
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

        // CHOOSE RANDOM SLOT TO REPLACE OTHERWISE
        int randomSlot = UtilityTools.RandomVarianceInt(0, items.Length - 1);
        items[randomSlot] = item;
    }

    public void RemoveFromInventory(int index) // FOR REMOVAL FROM MENU
    {
        items[index] = null;
    }

    public bool IsInventoryFull()
    {
        bool isInventoryFull = true;

        foreach (Item item in items) 
        { 
            if (item == null)
            {
                isInventoryFull = false;
                return isInventoryFull;
            }
        }
        return isInventoryFull;
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