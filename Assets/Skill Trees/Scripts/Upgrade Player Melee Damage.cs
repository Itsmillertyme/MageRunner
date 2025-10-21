using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Melee Damage")]

public class UpgradePlayerMeleeDamage : Upgrade
{
    [Tooltip("Amount to be added to the base value of the stat")]
    [SerializeField] private float increase;

    public override void Apply(Ability ability)
    {
        int amountToAdd = (int)(((Player)ability).MeleeDamage * increase);
        ((Player)ability).SetMeleeDamage(amountToAdd);
    }
}