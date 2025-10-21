using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Damage Over Time")]

public class UpgradeSpellDamageOverTime : Upgrade
{
    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                switch (spell)
                {
                    case ThunderlordsCascade tc:
                        tc.SetDamageOverTime(true);
                        break;
                    default:
                        break;
                }
                break;
        }
    }
}