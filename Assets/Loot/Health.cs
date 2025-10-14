using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Health")]

public class Health : Loot
{
    [Header("Unique Attributes")]
    [Tooltip("Amount to heal when picked up")]
    [SerializeField] private int healthDropped;

    public int HealAmount => healthDropped;
}