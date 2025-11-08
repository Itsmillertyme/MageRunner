using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Stamina")]

public class UpgradePlayerMaxStamina : Upgrade {
    [Tooltip("Percentage amount to be multiplied to the base value of the stat")]
    [SerializeField] private float increase;

    public override void Apply(Ability ability) {
        int amountToAddToMax = (int) (((Player) ability).MaxStamina * increase * 0.01f);
        ((Player) ability).SetMaxStamina(amountToAddToMax);
    }

}
