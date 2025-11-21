using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Damage Resistance")]

public class PerkDamageResistance : ItemPerk
{
    private float delta;

    public float Delta => delta;

    private void OnEnable()
    {
        abilityDelta = ModifyAbility.Player;
    }

    public override void SetDelta(float delta)
    {
        this.delta = delta;
    }

    public override void ApplyModifier(Ability ability)
    {
        Player player = (Player)ability;
        player.SetDamageResistance(delta);
    }

    public override void RemoveModifier(Ability ability)
    {
        Player player = (Player)ability;
        player.SetDamageResistance(-delta);
    }
}