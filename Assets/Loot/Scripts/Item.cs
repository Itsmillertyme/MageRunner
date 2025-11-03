using UnityEngine;

[CreateAssetMenu(menuName = "Loot Drops/Item")]

public class Item : Loot {
    [Header("Unique Attributes")]
    [SerializeField] private Rarity rarity;
    [SerializeField] private BodyPart bodyPart;
    [SerializeField] private Sprite itemIcon;

    public Rarity Rarity => rarity;
    public Sprite ItemIcon => itemIcon;

    private int perkCount;
    [Tooltip("1.0f = 100%")]
    private float perkBoostAmount;
    private int bodyPartIndex;

    public int BodyPartIndex => bodyPartIndex;

    private void OnEnable() {
        perkBoostAmount = SetRarityInfo();
    }

    private float SetRarityInfo() {
        float perkBoostAmount = 0;

        switch (rarity) {
            case Rarity.Legendary:
                perkCount = 4;
                perkBoostAmount = 0.15f;
                break;
            case Rarity.Exotic:
                perkCount = 3;
                perkBoostAmount = 0.10f;
                break;
            case Rarity.Rare:
                perkCount = 2;
                perkBoostAmount = 0.08f;
                break;
            case Rarity.Uncommon:
                perkCount = 1;
                perkBoostAmount = 0.05f;
                break;
            case Rarity.Common:
                perkCount = 1;
                perkBoostAmount = 0.02f;
                break;
        }

        bodyPartIndex = (int) bodyPart;
        float perkBoostFactor = perkBoostAmount / 2;
        perkBoostAmount += UtilityTools.RandomVarianceFloat(-0.5f, 0.5f);
        return perkBoostAmount * perkBoostFactor;
    }

    private void ChooseRandomPerks() {
        // boost player health, mana, stats, etc here
        // choose random amount of stats to boost

        for (int i = 0; i < perkCount; i++) {
            // i think we should do an upgrades class.
            // might be able to make an abstract one and make it work with the skill tree system better
        }
    }
}

public enum Rarity {
    Legendary,
    Exotic,
    Rare,
    Uncommon,
    Common
}

public enum BodyPart {
    Head,
    Torso,
    Arms,
    Legs,
    Feet
}