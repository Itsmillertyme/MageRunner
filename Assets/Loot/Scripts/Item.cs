using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Item")]

public class Item : Loot
{
    [Header("Unique Attributes")]
    [SerializeField] private Rarity rarity;
    private readonly int perkCountLegendary = 3;
    private readonly int perkCountExotic = 2;
    private readonly int perkCountRare = 2;
    private readonly int perkCountUncommon = 1;
    private readonly int perkCountCommon = 1;
    private readonly float perkMaxValueLegendary = 0.15f;
    private readonly float perkMaxValueExotic = 0.10f;
    private readonly float perkMaxValueRare = 0.075f;
    private readonly float perkMaxValueUncommon = 0.05f;
    private readonly float perkMaxValueCommon = 0.025f;

    [SerializeField] private ItemPerk[] itemPerkPool;
    [Header("UI")]
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private float iconShowDelay;
    [SerializeField] private string itemName;

    [SerializeField] private ItemPerk[] perks;
    [SerializeField] float[] perksDeltas;
    private int perkCount;
    private float perkDelta;

    public Rarity Rarity => rarity;
    public Sprite ItemIcon => itemIcon;
    public ItemPerk[] Perks => perks;
    public float[] PerksDeltas => perksDeltas;
    public float IconShowDelay => iconShowDelay;
    public string ItemName => itemName;

    public void SetItem(Rarity rarity, Sprite itemIcon, ItemPerk[] perks, float[] perksDeltas, string itemName)
    {
        this.rarity = rarity;
        this.itemIcon = itemIcon;
        this.perks = perks;
        this.perksDeltas = perksDeltas;
        this.itemName = itemName;
    }

    private void SetPerkAttributes()
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                perkCount = perkCountLegendary;
                perkDelta = perkMaxValueLegendary;
                break;
            case Rarity.Exotic:
                perkCount = perkCountExotic;
                perkDelta = perkMaxValueExotic;
                break;
            case Rarity.Rare:
                perkCount = perkCountRare;
                perkDelta = perkMaxValueRare;
                break;
            case Rarity.Uncommon:
                perkCount = perkCountUncommon;
                perkDelta = perkMaxValueUncommon;
                break;
            case Rarity.Common:
                perkCount = perkCountCommon;
                perkDelta = perkMaxValueCommon;
                break;
        }
    }

    public (ItemPerk[], float[]) SetPerks()
    {
        // SET ATTRIBUTES
        SetPerkAttributes();

        // CHOOSE PERKS
        (ItemPerk[], float[]) perkData = ChooseRandomPerks();
        perks = perkData.Item1;
        perksDeltas = perkData.Item2;
        return (perks, perksDeltas);
    }
        
    private (ItemPerk[], float[]) ChooseRandomPerks()
    {
        // ITEM 1
        ItemPerk[] perkList = new ItemPerk[perkCount];
        for (int i = 0; i < perkList.Length; i++)
        {
            int selection = UnityEngine.Random.Range(0, itemPerkPool.Length);
            perkList[i] = itemPerkPool[selection];
        }

        // ITEM 2
        float[] perksDelta = new float[perkCount];
        float perkBoostFactor = perkDelta / 2;
        for (int i = 0; i < perkList.Length; i++)
        {
            float value = UtilityTools.RandomVarianceFloat(-perkBoostFactor, 0);
            perkDelta += value;
            perksDelta[i] = Mathf.Round(perkDelta * 100f) / 100f;
        }
        return (perkList, perksDelta); // ITEM 1 AND 2
    }

    public void ApplyPerks()
    {
        foreach (ItemPerk perk in perks)
        {
            perk.Apply();
        }
    }

    public void RemovePerks()
    {
        foreach (ItemPerk perk in perks)
        {
            perk.Remove();
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