using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Loot Drop Luck")]

public class UpgradePlayerLootDropLuck : Upgrade {
    [Tooltip("Amount to be added to the base value of the stat")]
    [SerializeField] private float increase;

    public override void Apply(Ability ability) {
        int amountToAdd = (int) (((Player) ability).LootDropLuck * increase);
        ((Player) ability).SetLootDropLuck(amountToAdd + ((Player) ability).LootDropLuck);
    }
}