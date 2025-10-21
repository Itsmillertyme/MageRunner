using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Travel Speed")]

public class UpgradeSpellTravelSpeed : Upgrade
{
    [Tooltip("Amount to be added to the base value of the move speed")]
    [SerializeField] private float movementIncrease;
    [Header("If Shatterstone spell")]
    [SerializeField] private float riseIncrease;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                switch (spell)
                {
                    case ShatterstoneBarrage sb:
                        sb.SetRiseSpeed(sb.RiseSpeed + riseIncrease);
                        spell.SetMoveSpeed(spell.MoveSpeed + movementIncrease);
                        break;
                    default:
                        spell.SetMoveSpeed(spell.MoveSpeed + movementIncrease);
                        break;
                }
                break;
        }
    }
}