using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Player player;

    private Item[] items;
    private int currentCapacity = 1;
    private int maxCapacity = 5;

    public Item[] Items => items;

    private void Awake()
    {
        items = new Item[maxCapacity];
    }

    public void AddToInventory(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (item.BodyPartIndex == i)
            {
                items[i] = item;
            }
            
        }
    }

    public void RemoveFromInventory(Item item)
    {

    }

    public void IncreaseCapacity()
    {
        if (currentCapacity == maxCapacity)
        {
            return;
        }
        currentCapacity++;
    }
}