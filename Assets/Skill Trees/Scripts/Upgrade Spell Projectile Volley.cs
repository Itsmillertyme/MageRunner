using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Projectiles Volley")]

public class UpgradeSpellProjectileVolley : Upgrade
{
    [Tooltip("Amount to be added to the base value of the amount of volleys of projectiles")]
    [SerializeField] private int increase;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                switch (spell)
                {
                    case ThunderlordsCascade tc:
                        tc.SetVolleyCount(tc.VolleyCount + increase);
                        break;
                    default:
                        break;
                }
                break;
        }
    }
}