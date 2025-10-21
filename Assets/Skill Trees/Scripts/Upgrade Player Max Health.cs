using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Health")]

public class UpgradePlayerMaxHealth : Upgrade
{
    [Tooltip("Percentage amount to be multiplied to the base value of the stat")]
    [SerializeField] private float increase;

    public override void Apply(Ability ability)
    {
        int amountToAddToMax = (int)(((Player)ability).MaxHealth * increase);
        ((Player)ability).SetMaxHealth(amountToAddToMax);
    }
}