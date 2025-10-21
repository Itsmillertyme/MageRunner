using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Projectile Size")]

public class UpgradeSpellProjectileSize : Upgrade
{
    [Header("Only edit one of the two attributes")]
    [Tooltip("Amount to be added to the base value")]
    [SerializeField] private Vector3 sizeIncreaseVector3;
    [SerializeField] private float sizeIncreaseFloat;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:

                switch (spell)
                {
                    case ShatterstoneBarrage sb:
                        sb.SetProjectileSizeScalar(sb.ProjectileSizeScalar + sizeIncreaseFloat);
                        break;
                    default:
                        spell.SetProjectileSize(spell.ProjectileSize + sizeIncreaseVector3);
                        break;
                }
                break;
        }
    }
}