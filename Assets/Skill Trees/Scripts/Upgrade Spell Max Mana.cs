using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spells/Mana Increase")]

public class UpgradSpellMaxMana : Upgrade
{
    [Tooltip("Amount to be added to the base value of mana capacity")]
    [SerializeField] private int increase;

    public override void Apply(Ability ability)
    {
        switch (ability)
        {
            case Spell spell:
                spell.SetMaxMana(spell.CurrentMana + increase);
                break;
        }
    }
}