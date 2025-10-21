using UnityEngine;

public class GuaranteedLootDropper : MonoBehaviour
{
    [SerializeField] private Item[] loot;
    private LootSystem lootDropper;

    private void Awake()
    {
        lootDropper = FindFirstObjectByType<LootSystem>();
    }

    private void OnDestroy()
    {
        DropLoot();
    }

    private void DropLoot()
    {
        foreach (Item item in loot)
        {
            lootDropper.SpawnNonRandomLoot(item, transform.position);
        }
    }
}