using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Damage")]

public class UpgradeSpellDamage : Upgrade
{
    [Tooltip("Amount to be added to the base value of damage")]
    [SerializeField] private int increase;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                spell.SetDamage(spell.Damage + increase);
                break;
        }
    }
}