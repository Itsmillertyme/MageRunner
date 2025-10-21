using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Healing Efficiency")]

public class UpgradePlayerHealingEfficiency : Upgrade
{
    [Tooltip("Amount to be added to the base value of the stat")]
    [SerializeField] private float increase;

    public override void Apply(Ability ability)
    {
        int amountToAdd = (int)(((Player)ability).HealingEfficiency * increase);
        ((Player)ability).SetHealingEfficiency(amountToAdd);
    }
}