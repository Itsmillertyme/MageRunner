using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Projectiles Count")]

public class UpgradeSpellProjectilesCount : Upgrade
{
    [Tooltip("Amount to be added to the base value of the amount of projectiles")]
    [SerializeField] private int increase;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:

                switch (spell)
                {
                    case ShatterstoneBarrage sb:
                        sb.SetProjectileCount(sb.ProjectileCount + increase);
                        break;
                    case ThunderlordsCascade tc:
                        tc.SetProjectileCount(tc.ProjectileCount + increase);
                        break;
                }
                break;
        }
    }
}