using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    [Header("Loot Pools")]
    [SerializeField] private Health[] healthLootPool;
    [SerializeField] private Item[] itemLootPool;
    [SerializeField] private Mana[] manaLootPool;


    [Header("Loot Drop Settings")]
    [Tooltip("The maximum amount of mana drops that can drop at one time")]
    [SerializeField] private int healthLootDropMax;
    [SerializeField] private int manaLootDropMax;
    [SerializeField] private int itemLootDropMax;

    [Header("References")]
    [SerializeField] private Player player;

    private readonly List<Loot> lootToDrop = new();

    public void SelectThenDropLoot(Vector3 location)
    {
        // GET HEALTH DROPS TO DROP
        Health[] healthDrops = GetHealthLootToDrop();
        if (healthDrops != null)
        {
            foreach (Health health in healthDrops)
            {
                lootToDrop.Add(health);
            }
        }

        // GET ITEM DROPS TO DROP
        Item[] itemDrops = GetItemsToDrop();
        if (itemDrops != null)
        {
            foreach (Item item in itemDrops)
            {
                lootToDrop.Add(item);
            }
        }

        // GET MANA DROPS TO DROP
        Mana[] manaDrops = GetManaLootToDrop();
        if (manaDrops != null)
        {
            foreach (Mana mana in manaDrops)
            {
                lootToDrop.Add(mana);
            }
        }

        // SPAWN THE LOOT
        foreach (Loot loot in lootToDrop)
        { 
            SpawnLoot(loot, location);
        }

        // CLEAR AFTER FINISHING LOOT DROP
        lootToDrop.Clear();
    }

    private Health[] GetHealthLootToDrop()
    {
        int lootDropCount = Random.Range(1, healthLootDropMax + 1); // 0 - healthLootDropMax
        List<Health> eligibleToSpawn = new();

        // IF RNG VALUE IS IN RANGE ADD TO ELIGIBLE FOR SPAWN
        foreach (Health health in healthLootPool)
        {
            if (RandomNumber() < health.DropChance)
            {
                eligibleToSpawn.Add(health);
            }
        }

        // IF LIST OF ELIGBLE LOOT IS NOT EMPTY
        if (eligibleToSpawn.Count > 0)
        {
            // CREATE ARRAY AND FILL IT WITH LOOT TO DROP
            Health[] healthDrops = new Health[lootDropCount];
            for (int i = 0; i < healthDrops.Length; i++)
            {
                // CHOOSE LOOT FOR EACH DROP SLOT
                healthDrops[i] = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            }
            return healthDrops;
        }
        return null;
    }

    private Item[] GetItemsToDrop()
    {
        int lootDropCount = Random.Range(1, itemLootDropMax + 1); // 0 - itemLootDropMax
        List<Item> eligibleToSpawn = new();

        // IF RNG VALUE IS IN RANGE ADD TO ELIGIBLE FOR SPAWN
        foreach (Item item in itemLootPool)
        {
            if (RandomNumber() < item.DropChance)
            {
                eligibleToSpawn.Add(item);
            }
        }

        // IF LIST OF ELIGBLE LOOT IS NOT EMPTY
        if (eligibleToSpawn.Count > 0)
        {
            // CREATE ARRAY AND FILL IT WITH LOOT TO DROP
            Item[] itemDrops = new Item[lootDropCount];
            for (int i = 0; i < itemDrops.Length; i++)
            {
                // CHOOSE LOOT FOR EACH DROP SLOT
                itemDrops[i] = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            }
            return itemDrops;
        }
        return null;
    }

    private Mana[] GetManaLootToDrop()
    {
        int lootDropCount = Random.Range(1, manaLootDropMax + 1); // 0 - manaLootDropMax
        List<Mana> eligibleToSpawn = new();

        // IF RNG VALUE IS IN RANGE ADD TO ELIGIBLE FOR SPAWN
        foreach (Mana mana in manaLootPool)
        {
            if (RandomNumber() < mana.DropChance)
            {
                eligibleToSpawn.Add(mana);
            }
        }

        // IF LIST OF ELIGBLE LOOT IS NOT EMPTY
        if (eligibleToSpawn.Count > 0)
        {
            // CREATE ARRAY AND FILL IT WITH LOOT TO DROP
            Mana[] manaDrops = new Mana[lootDropCount];
            for (int i = 0; i < manaDrops.Length; i++)
            {
                // CHOOSE LOOT FOR EACH DROP SLOT
                manaDrops[i] = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            }
            return manaDrops;
        }
        return null;
    }

    private void SpawnLoot(Loot loot, Vector3 position) {
        GameObject lootInstance = Instantiate(loot.LootDrop, position, Quaternion.identity);

        //Setup loot UI
        if (loot is Item) {
            Item item = (Item) loot;
            ItemUIController itemUI = lootInstance.GetComponentInChildren<ItemUIController>();
            itemUI.Initialize(item.Rarity, item.ItemName, item.ItemIcon, "buff text", "debuff text");
        }
    }

    public void SpawnNonRandomLoot(Loot loot, Vector3 position)
    {
        Instantiate(loot.LootDrop, position, Quaternion.identity);
    }
        
    private int RandomNumber()
    {
        int dropChance = Random.Range(0, 101); // 0 - 100
        dropChance += (int)(dropChance * player.LootDropLuck);
        return dropChance;
    }
}