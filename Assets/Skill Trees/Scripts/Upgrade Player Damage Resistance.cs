using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Damage Resistance")]

public class PlayerUpgradeDamageResistance : Upgrade
{
    [Tooltip("Amount to be added to the base value of the stat")]
    [SerializeField] private float increase;

    public override void Apply(Ability ability)
    {
        int amountToAdd = (int)(((Player)ability).DamageResistance * increase);
        ((Player)ability).SetDamageResistance(amountToAdd);
    }
}