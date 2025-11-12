using UnityEngine;

public class LootItemMenuController : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private GameObject itemParent;
    [SerializeField] private Item loot;
    [SerializeField] private GameObject buttonLayout1; // EQUIP BUTTON
    [SerializeField] private GameObject buttonLayout2; // REPLACE AND COMPARE BUTTONS

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory>();
        if (!inventory.IsInventoryFull()) // IF SLOT IS EMPTY
        {
            buttonLayout1.SetActive(true);
        }
        else // IF SLOT OCCUPIED
        {
            buttonLayout2.SetActive(true);
        }
    }

    public void AddItemToInventory()
    {
        inventory.AddToInventory(loot);
        Destroy(itemParent);
    }
}