using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Damage Increase Specific Spell")]

public class PerkDamageIncreaseSpecificSpell : ItemPerk
{
    private float delta;

    public float Delta => delta;

    private void OnEnable()
    {
        abilityDelta = ModifyAbility.Spellbook;
    }

    public override void SetDelta(float delta)
    {
        this.delta = delta;
    }

    public override void ApplyModifier(Ability ability)
    {
        Debug.Log(((Spell)ability).Damage);
        ((Spell)ability).SetDamage((int)delta);

        Debug.Log((Spell)ability);
        Debug.Log(((Spell)ability).Damage);
    }

    public override void RemoveModifier(Ability ability)
    {
        ((Spell)ability).SetDamage((int)-delta);
    }
}