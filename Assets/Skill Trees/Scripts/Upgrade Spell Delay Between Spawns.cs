using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Delay Between Spawns")]

public class UpgradeSpellDelayBetweenSpawns : Upgrade
{
    [Tooltip("Amount to be reduced from the base value of the amount of time it takes to spawn projectiles")]
    [SerializeField] private float reduction;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                switch (spell)
                {
                    case ShatterstoneBarrage sb:
                        sb.SetDelayBetweenSpawns(sb.DelayBetweenSpawns - reduction);
                        break;
                }
                break;
        }
    }
}