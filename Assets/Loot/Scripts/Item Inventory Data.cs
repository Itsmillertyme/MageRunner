using UnityEngine;

public class ItemInventoryData
{
    private readonly Rarity rarity;
    private readonly Sprite icon;
    private readonly ItemPerk[] perks;
    private readonly float[] perksDeltas;
    private readonly string itemName;

    public Rarity ItemRarity => rarity;
    public Sprite ItemIcon => icon;
    public ItemPerk[] Perks => perks;
    public float[] PerksDeltas => perksDeltas;
    public string ItemName => itemName;

    public ItemInventoryData(Rarity rarity, Sprite icon, ItemPerk[] perks, float[] perksDeltas, string itemName)
    {
        this.rarity = rarity;
        this.icon = icon;
        this.perks = perks;
        this.perksDeltas = perksDeltas;
        this.itemName = itemName;
    }
}