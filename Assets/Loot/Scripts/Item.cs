using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Item")]

public class Item : Loot
{
    [Header("Unique Attributes")]
    [SerializeField] private Rarity rarity;
    [SerializeField] private int perkCountLegendary = 3;
    [SerializeField] private int perkCountExotic = 2;
    [SerializeField] private int perkCountRare = 2;
    [SerializeField] private int perkCountUncommon = 1;
    [SerializeField] private int perkCountCommon = 1;
    [SerializeField] private float perkMaxValueLegendary = 0.15f;
    [SerializeField] private float perkMaxValueExotic = 0.10f;
    [SerializeField] private float perkMaxValueRare = 0.075f;
    [SerializeField] private float perkMaxValueUncommon = 0.5f;
    [SerializeField] private float perkMaxValueCommon = 0.25f;

    [SerializeField] private ItemPerk[] itemPerkPool;
    [Header("UI")]
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private float iconShowDelay;
    [SerializeField] private string itemName;

    private ItemPerk[] perks;
    private int perkCount;
    private float perkBoostAmount;

    public Rarity Rarity => rarity;
    public Sprite ItemIcon => itemIcon;
    public ItemPerk[] Perks => perks;
    public float IconShowDelay => iconShowDelay;
    public string ItemName => itemName;

    private void SetPerkAttributes()
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                perkCount = perkCountLegendary;
                perkBoostAmount = perkMaxValueLegendary;
                break;
            case Rarity.Exotic:
                perkCount = perkCountExotic;
                perkBoostAmount = perkMaxValueExotic;
                break;
            case Rarity.Rare:
                perkCount = perkCountRare;
                perkBoostAmount = perkMaxValueRare;
                break;
            case Rarity.Uncommon:
                perkCount = perkCountUncommon;
                perkBoostAmount = perkMaxValueUncommon;
                break;
            case Rarity.Common:
                perkCount = perkCountCommon;
                perkBoostAmount = perkMaxValueCommon;
                break;
        }
    }

    public (ItemPerk[], float[]) SetPerks()
    {
        // SET ATTRIBUTES
        SetPerkAttributes();

        // CHOOSE PERKS
        perks = ChooseRandomPerks().Item1;
        float[] perkValues = ChooseRandomPerks().Item2;
        return (perks, perkValues);
    }
        
    private (ItemPerk[], float[]) ChooseRandomPerks()
    {
        ItemPerk[] perkList = new ItemPerk[perkCount];
        for (int i = 0; i < perkList.Length; i++)
        {
            int selection = Random.Range(0, itemPerkPool.Length);
            perkList[i] = itemPerkPool[selection];
        }

        float[] perkValues = new float[perkCount];
        for (int i = 0; i < perkList.Length; i++)
        {
            float perkBoostFactor = perkBoostAmount / 2;
            perkBoostAmount += UtilityTools.RandomVarianceFloat(-perkBoostFactor, 0);
        }
        return (perkList, perkValues);
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