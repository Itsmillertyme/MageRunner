using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Item")]

public class Item : Loot
{
    [Header("Item Generation Values")]
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
    [SerializeField] private Sprite[] iconPool;
    [Header("UI")]
    [SerializeField] private Sprite itemIcon; // make this be randomly selected from icon pool
    [SerializeField] private string itemName; // set item name to be the name of the sprite icon plus a modifier for the greatest valued stat. 

    [SerializeField] private ItemPerk[] perks;
    private float[] perksDeltas;
    private Player player;
    private SpellBook spellBook;
    private int perkCount;
    private float maxPerkDelta;

    public Rarity Rarity => rarity;
    public Sprite ItemIcon => itemIcon;
    public ItemPerk[] Perks => perks;
    public string ItemName => itemName;

    private void OnEnable()
    {
        SetPerkAttributes();
    }

    public void SetItem(Item item)
    {
        this.rarity = item.Rarity;
        this.itemIcon = item.ItemIcon;
        this.perks = item.perks;
        this.perksDeltas = item.perksDeltas;
        this.itemName = item.itemName;

        player = PlayerAbilities.Instance.PlayerSO;
        spellBook = SpellBook.Instance;
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

        // PERK SELECTION
        for (int i = 0; i < perks.Length; i++)
        {
            int selection = Random.Range(0, itemPerkPool.Length);
            perks[i] = Instantiate(itemPerkPool[selection]);
        }

        // PERK DELTA SELECTION
        float halfDelta = maxPerkDelta / 2;
        for (int i = 0; i < perks.Length; i++)
        {
            float variance = UtilityTools.RandomVarianceFloat(-halfDelta, 0, 4);
            int sign = UtilityTools.RandomVarianceInt(0, 1);  // SET SIGN VALUE. 0 FOR POSITIVE, 1 FOR NEGATIVE.
            float finalValue = maxPerkDelta + variance;
            perksDeltas[i] = (sign == 1) ? -finalValue : finalValue;
            perks[i].SetDelta(perksDeltas[i]);
        }
    }

    public void ApplyPerks()
    {
        foreach (ItemPerk perk in perks)
        {
            if (perk.AbilityDelta == ModifyAbility.Player)
            {
                perk.ApplyModifier(player);
            }
            else 
            {
                int selection = UtilityTools.RandomVarianceInt(0, spellBook.AllSpells.Length - 1); // CHOOSE RANDOM SPELL TO MODIFY

                DeveloperScript.Instance.debug($"{spellBook.AllSpells[selection]}", true);
                perk.ApplyModifier(spellBook.AllSpells[selection]);
            }
        }
    }

    public void RemovePerks()
    {
        foreach (ItemPerk perk in perks)
        {
            if (perk.AbilityDelta == ModifyAbility.Player)
            {
                perk.RemoveModifier(player);
            }
            else
            {
                int selection = UtilityTools.RandomVarianceInt(0, spellBook.AllSpells.Length - 1); // CHOOSE RANDOM SPELL TO MODIFY
                perk.RemoveModifier(spellBook.AllSpells[selection]);
            }
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