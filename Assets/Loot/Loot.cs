using UnityEngine;

public class Loot : ScriptableObject
{
    [Header("Loot Attributes")]
    [SerializeField] private string itemName;
    [Tooltip("0 - 100 (Whole Numbers")]
    [SerializeField] private int dropChance;
    [Tooltip("Prefab")]
    [SerializeField] private GameObject lootDrop;

    public int DropChance => dropChance;
    public GameObject LootDrop => lootDrop;
}