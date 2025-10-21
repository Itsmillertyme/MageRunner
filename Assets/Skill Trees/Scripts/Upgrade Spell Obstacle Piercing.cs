using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Obstacle Piercing")]

public class UpgradeSpellObstaclePiercing : Upgrade
{
    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                spell.SetDestroyOnEnemyImpact(false);
                spell.SetDestroyOnEnvironmentalImpact(false);
                break;
        }
    }
}