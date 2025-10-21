using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Fire Rate")]

public class UpgradeSpellFireRate : Upgrade
{
    [Tooltip("Amount to be subtracted from the base value of cast cooldown time")]
    [SerializeField] private float reduction;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                spell.SetCastCooldownTime(spell.CastCooldownTime - reduction);
                break;
        }
    }
}