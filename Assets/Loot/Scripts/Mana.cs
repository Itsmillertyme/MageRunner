using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Mana")]

public class Mana : Loot
{
    [Header("Unique Attributes")]
    [SerializeField] private string itemName;
    [Tooltip("Max amount to recover when picked up")]
    [SerializeField] private int maxManaDropped;
    [SerializeField] private Spell matchingSpell;
    [SerializeField] private float pursuitRange;
    [SerializeField] private float pursuitSpeed;
    [SerializeField] private float pursuitDelay; // HOW LONG AFTER FLING BEFORE RB IS TURNED OFF
    [SerializeField] private float minLocalScale;

    public int MaxManaDropped => maxManaDropped;
    public Spell SpellToReplenish => matchingSpell;
    public float PursuitRange => pursuitRange;
    public float PursuitSpeed => pursuitSpeed;
    public float PursuitDelay => pursuitDelay;
    public float MinManaScale => minLocalScale;
}