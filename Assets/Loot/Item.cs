using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Item")]

public class Item : Loot
{
    [Header("Unique Attributes")]
    [SerializeField] private Rarity rarity;

    private int perkCount;

    private void ChooseRandomBoosts()
    {
        // boost player health, mana, stats, etc here
        // choose random amount of stats to boost

        for (int i = 0; i < perkCount; i++)
        {
            // i think we should do an upgrades class.
            // might be able to make an abstract one and make it work with the skill tree system better
        }
    }



    private void OnEnable()
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                perkCount = 4;
                break;
            case Rarity.Exotic:
                perkCount = 3;
                break;
            case Rarity.Rare:
                perkCount = 2;
                break;
            case Rarity.Uncommon:
                perkCount = 1;
                break;
            case Rarity.Common:
                perkCount = 0;
                break;
        }
    }
}

public enum Rarity
{
    Legendary,
    Exotic,
    Rare,
    Uncommon,
    Common
}