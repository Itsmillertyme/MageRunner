using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Mana")]

public class Mana : Loot
{
    [Header("Unique Attributes")]
    [Tooltip("Amount to recover when picked up")]
    [SerializeField] private int manaDropped;

    public int ManaRecoverAmount => manaDropped;
}