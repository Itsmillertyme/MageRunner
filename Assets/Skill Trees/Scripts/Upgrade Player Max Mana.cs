using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player/Mana")]

public class UpgradePlayerMaxMana : Upgrade
{
    [Tooltip("Percentage amount to be multiplied to the base value of the stat")]
    [SerializeField] private float increase;

    private SpellBook spellBook;

    private void OnEnable()
    {
        spellBook = FindFirstObjectByType<SpellBook>();
    }

    public override void Apply(Ability ability)
    {
        foreach (Spell spell in spellBook.AllSpells)
        {
            int amountToAdd = (int)(spell.MaxMana * increase);
            spell.SetMaxMana(amountToAdd);
        }
    }
}