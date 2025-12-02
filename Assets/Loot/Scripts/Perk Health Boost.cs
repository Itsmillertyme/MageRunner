using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Perk Health Boost")]
public class PerkHealthBoost : ItemPerk
{
    private float delta;
    private float playerAppliedBoost;

    private void OnEnable()
    {
        abilityDelta = ModifyAbility.Player;
        perkName = "Healing Efficieny";
    }

    public override void ApplyModifier(Ability ability)
    {
        Player player = ability as Player;
        float boost = delta;
        boost *= player.HealingEfficiency;
        player.SetHealingEfficiency(boost);
        playerAppliedBoost = boost;
    }

    public override string GetPerkDeltaRounded()
    {
        float formatted = delta;
        string sign = formatted > 0 ? "+" : ""; // NO NEED TO PUT THE MINUS SINCE IT'S ALREADY IN THE FLOAT
        formatted *= 100f;

        if (sign == "+")
        {
            string formattedText = $"<color=green>{sign}{formatted:F2}</color>";
            return formattedText;
        }
        else // IF NEGATIVE CHANGE COLOR OF TEXT
        {
            string formattedText = $"<color=red>{sign}{formatted:F2}</color>";
            return formattedText;
        }
    }

    public override void RemoveModifier(Ability ability)
    {
        Player player = ability as Player;
        player.SetHealingEfficiency(-playerAppliedBoost);
    }

    public override void SetDelta(float delta)
    {
        this.delta = delta;
    }
}
