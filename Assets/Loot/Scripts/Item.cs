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
    private float[] perksDeltas;
    private Player player;
    private SpellBook spellBook;
    private int perkCount;
    private float maxPerkDelta;

    public Rarity Rarity => rarity;
    public Sprite ItemIcon => itemIcon;
    public ItemPerk[] Perks => perks;
    public float[] PerksDeltas => perksDeltas;
    public float IconShowDelay => iconShowDelay;
    public string ItemName => itemName;

    private void OnEnable()
    {
        SetPerkAttributes();
        player = FindFirstObjectByType<PlayerAbilities>().PlayerSO;
        spellBook = FindFirstObjectByType<SpellBook>();
    }

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
                maxPerkDelta = perkMaxValueLegendary;
                break;
            case Rarity.Exotic:
                perkCount = perkCountExotic;
                maxPerkDelta = perkMaxValueExotic;
                break;
            case Rarity.Rare:
                perkCount = perkCountRare;
                maxPerkDelta = perkMaxValueRare;
                break;
            case Rarity.Uncommon:
                perkCount = perkCountUncommon;
                maxPerkDelta = perkMaxValueUncommon;
                break;
            case Rarity.Common:
                perkCount = perkCountCommon;
                maxPerkDelta = perkMaxValueCommon;
                break;
        }
    }
        
    public void ChooseRandomPerks()
    {
        perks = new ItemPerk[perkCount];
        perksDeltas = new float[perkCount];

        // ITEM 1
        for (int i = 0; i < perks.Length; i++)
        {
            int selection = Random.Range(0, itemPerkPool.Length);
            perks[i] = Instantiate(itemPerkPool[selection]);
        }

        // ITEM 2
        float halfDelta = maxPerkDelta / 2;
        for (int i = 0; i < perks.Length; i++)
        {
            float variance = UtilityTools.RandomVarianceFloat(-halfDelta, 0);
            int sign = UtilityTools.RandomVarianceInt(0, 1);  // SET SIGN VALUE. 0 FOR POSITIVE, 1 FOR NEGATIVE.
            float finalValue = maxPerkDelta + variance;
            perksDeltas[i] = (sign == 1) ? -finalValue : finalValue;
            perks[i].SetDelta(perksDeltas[i]);
        }
    }

    public void ApplyPerks()
    {
        string msg = "";
        foreach (ItemPerk perk in perks)
        {
            if (perk.AbilityDelta == ModifyAbility.Player)
            {
                perk.ApplyModifier(player);
            }
            else 
            {
                int selection = UtilityTools.RandomVarianceInt(0, spellBook.AllSpells.Length - 1); // CHOOSE RANDOM SPELL TO MODIFY
                perk.ApplyModifier(spellBook.AllSpells[selection]);
            }

            msg += $"Added {perk.name} with delta {((PerkDamageResistance)perk).Delta}\n";
        }
        DeveloperScript.Instance.debug(msg, true); // DELETE PUBLIC GETTER FOR PERK DELTA
    }

    public void RemovePerks()
    {
        string msg = "";
        foreach (ItemPerk perk in perks)
        {
            perk.RemoveModifier(player);
            msg += $"Removed {perk.name} with delta {((PerkDamageResistance)perk).Delta}\n";
        }
        DeveloperScript.Instance.debug(msg, true); // DELETE PUBLIC GETTER FOR PERK DELTA
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