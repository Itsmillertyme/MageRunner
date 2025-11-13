using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Damage Resistance")]

public class PerkDamageResistance : ItemPerk
{
    private float delta;

    public override void OnEnable()
    {
        // DO NOTHING
    }

    public override void Set(float delta)
    {
        this.delta = Mathf.Round(delta * 100f) / 100f;
    }

    public override void Apply()
    {
        player.SetDamageResistance(delta);
    }

    public override void Remove()
    {
        player.SetDamageResistance(-delta);
    }
}