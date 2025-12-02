using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Item Names Key")]
public static class ItemNames
{
    public static string GetItemName(ItemPerk perk)
    {
        string modifier = "";
        switch (perk)
        {
            case PerkDamageIncreaseSpecificSpell:
                modifier = "Complete this logic later";  // FIX THIS ONCE THE INDIVIDUAL SPELL LOGIC HAS BEEN BUILT OUT IN THE ITEM PERK CUSTOM CLASS. 
                break;
            case PerkDamageResistance:
                modifier = "Resilient";
                break;
            case PerkHealthBoost:
                modifier = "Medicinal";
                break;
            // health boost
            // stamina boost
            // melee damage
            // all spell damage at a reudced damage



        }
        return $"{modifier}";
    }
}