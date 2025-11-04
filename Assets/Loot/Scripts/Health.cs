using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Health")]

public class Health : Loot
{
    [Header("Unique Attributes")]
    [Tooltip("Amount to heal when picked up")]
    [SerializeField] private int maxHealthDropped;
    [SerializeField] private float minLocalScale;

    public int MaxHealthDropped => maxHealthDropped;
    public float MinLocalScale => minLocalScale;
}