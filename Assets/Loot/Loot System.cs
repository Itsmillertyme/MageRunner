using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    [Header("Loot Pools")]
    [SerializeField] Item[] itemLootPool;
    [SerializeField] Mana[] manaLootPool;

    [Header("Loot Drop Settings")]
    [SerializeField] int manaDropMax;

    private readonly List<Loot> lootToDrop = new();

    public void DropLoot(Vector3 location)
    {
        // GET ITEM TO DROP
        Item itemDrop = GetItemToDrop();

        if (itemDrop != null)
        {
            lootToDrop.Add(itemDrop);
        }

        // GET MANA DROPS TO DROP
        Mana[] manaDrops = GetManaToDrop();

        if (manaDrops != null)
        {
            foreach (Mana mana in manaDrops)
            {
                if (mana != null)
                {
                    lootToDrop.Add(mana);
                }
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

    private Item GetItemToDrop()
    {
        int randomNumber = Random.Range(0, 101); // 0 - 100
        List<Item> eligibleToSpawn = new();


        foreach (Item item in itemLootPool)
        {
            if (randomNumber <= item.DropChance)
            {
                eligibleToSpawn.Add(item);
            }
        }

        if (eligibleToSpawn.Count > 0)
        {
            Item itemDrop = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            return itemDrop;
        }

        return null;
    }

    private Mana[] GetManaToDrop()
    {
        int randomNumber = Random.Range(0, 101); // 0 - 100
        int dropCount = Random.Range(1, manaDropMax + 1); // 0 - manaDropMax STACKS OF MANA
        List<Mana> eligibleToSpawn = new();

        foreach (Mana mana in manaLootPool)
        {
            if (randomNumber <= mana.DropChance)
            {
                eligibleToSpawn.Add(mana);
            }
        }

        if (eligibleToSpawn.Count > 0)
        {
            Mana[] manaDrops = new Mana[dropCount];

            for (int i = 0; i < dropCount; i++)
            {
                manaDrops[i] = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            }

            return manaDrops;
        }

        return null;
    }

    private void SpawnLoot(Loot loot, Vector3 position)
    {
        Instantiate(loot.LootDrop, position, Quaternion.identity);
    }
}