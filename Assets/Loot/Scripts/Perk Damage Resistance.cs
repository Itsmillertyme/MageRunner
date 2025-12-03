using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Damage Resistance")]

public class PerkDamageResistance : ItemPerk {
    private float delta;
    private float playerAppliedResistance;

    //public float Delta => delta;

    private void OnEnable() {
        abilityDelta = ModifyAbility.Player;
        perkName = "Damage Resistance";
    }

    public override void SetDelta(float delta) {
        this.delta = delta;
    }

    public override void ApplyModifier(Ability ability) {
        // WIP - COMMENTED OUT FOR DEMO
        //Player player = ability as Player;
        //float resistance = delta;
        //resistance *= player.DamageResistance;
        //player.SetDamageResistance(resistance);
        //playerAppliedResistance = resistance;
    }

    public override void RemoveModifier(Ability ability) {
        Player player = ability as Player;
        player.SetDamageResistance(-playerAppliedResistance);
    }

    public override string GetPerkDeltaRounded() {
        float formatted = delta;
        string sign = formatted > 0 ? "+" : ""; // NO NEED TO PUT THE MINUS SINCE IT'S ALREADY IN THE FLOAT
        formatted *= 100f;

        if (sign == "+") {
            string formattedText = $"<color=green>{sign}{formatted:F2}</color>";
            return formattedText;
        }
        else // IF NEGATIVE CHANGE COLOR OF TEXT
        {
            string formattedText = $"<color=red>{sign}{formatted:F2}</color>";
            return formattedText;
        }
    }
}