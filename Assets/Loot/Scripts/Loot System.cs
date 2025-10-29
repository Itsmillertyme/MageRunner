using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour {
    [Header("Loot Pools")]
    [SerializeField] private Health[] healthLootPool;
    [SerializeField] private Item[] itemLootPool;
    [SerializeField] private Mana[] manaLootPool;
    [SerializeField] private Upgrade[] itemPerkPool;

    [Header("Loot Drop Settings")]
    [Tooltip("The maximum amount of mana drops that can drop at one time")]
    [SerializeField] private int manaDropMax;

    [Header("References")]
    [SerializeField] private Player player;

    private readonly List<Loot> lootToDrop = new();

    public void DropLoot(Vector3 location) {
        // GET ITEM TO DROP
        Item itemDrop = GetItemToDrop();

        if (itemDrop != null) {
            lootToDrop.Add(itemDrop);
        }

        // GET MANA DROPS TO DROP
        Mana[] manaDrops = GetManaToDrop();

        if (manaDrops != null) {
            foreach (Mana mana in manaDrops) {
                if (mana != null) {
                    lootToDrop.Add(mana);
                }
            }
        }

        // SPAWN THE LOOT
        foreach (Loot loot in lootToDrop) {
            SpawnLoot(loot, location);
        }

        // CLEAR AFTER FINISHING LOOT DROP
        lootToDrop.Clear();
    }

    private Item GetItemToDrop() {
        List<Item> eligibleToSpawn = new();

        foreach (Item item in itemLootPool) {
            if (RandomNumber() <= item.DropChance) {
                eligibleToSpawn.Add(item);
            }
        }

        if (eligibleToSpawn.Count > 0) {
            Item itemDrop = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            return itemDrop;
        }

        return null;
    }

    private Mana[] GetManaToDrop() {
        int dropCount = Random.Range(1, manaDropMax + 1); // 0 - manaDropMax STACKS OF MANA
        List<Mana> eligibleToSpawn = new();

        foreach (Mana mana in manaLootPool) {
            if (RandomNumber() <= mana.DropChance) {
                eligibleToSpawn.Add(mana);
            }
        }

        if (eligibleToSpawn.Count > 0) {
            Mana[] manaDrops = new Mana[dropCount];

            for (int i = 0; i < dropCount; i++) {
                manaDrops[i] = eligibleToSpawn[Random.Range(0, eligibleToSpawn.Count)];
            }

            return manaDrops;
        }

        return null;
    }

    private void SpawnLoot(Loot loot, Vector3 position) {
        Instantiate(loot.LootDrop, position, Quaternion.identity);
    }

    public void SpawnNonRandomLoot(Loot loot, Vector3 position) {
        Instantiate(loot.LootDrop, position, Quaternion.identity);
    }

    private int RandomNumber() {
        int dropChance = Random.Range(0, 101); // 0 - 100
        dropChance += (int) (dropChance * player.LootDropLuck);
        return dropChance;
    }
}