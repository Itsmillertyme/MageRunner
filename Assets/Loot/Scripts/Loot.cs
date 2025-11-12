using UnityEngine;

public class Loot : ScriptableObject {
    [Header("Loot Attributes")]
    [Tooltip("Percentage")]
    [Range(0, 100)]
    [SerializeField] private int dropChance;
    [SerializeField] private float lifeSpan;

    [Header("Forces On Loot Spawn")]
    [SerializeField] private float upwardForce = 10f;
    [SerializeField] private float outwardForce = 5f;
    [SerializeField] private float torqueStrength = 5f;

    [Tooltip("Prefab")]
    [SerializeField] private GameObject lootDrop;

    public int DropChance => dropChance;
    public float LifeSpan => lifeSpan;
    public GameObject LootDrop => lootDrop;
    public float UpwardForce => upwardForce;
    public float OutwardForce => outwardForce;
    public float TorqueStrength => torqueStrength;
}