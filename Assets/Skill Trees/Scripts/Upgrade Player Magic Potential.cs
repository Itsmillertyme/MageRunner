using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/Player/Magic")]
public class UpgradePlayerMagicPotential : Upgrade {
    [Tooltip("Percentage amount to be multiplied to the base value of the stat")]
    [SerializeField] private float increase;

    private SpellBook spellBook;

    private void OnEnable() {
        spellBook = FindFirstObjectByType<SpellBook>();
    }

    public override void Apply(Ability ability) {
        foreach (Spell spell in spellBook.AllSpells) {
            int amountToDecrease = (int) (spell.CastCooldownTime * increase);
            spell.SetCastCooldownTime(spell.CastCooldownTime - amountToDecrease);
        }
    }
}
