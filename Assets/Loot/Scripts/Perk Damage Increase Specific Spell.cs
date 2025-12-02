using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Damage Increase Specific Spell")]




/// incomplete
/// 













public class PerkDamageIncreaseSpecificSpell : ItemPerk
{
    private float delta;

    public float Delta => delta;

    private void OnEnable()
    {
        abilityDelta = ModifyAbility.Spellbook;
        // get reference to the ui controller
        // get randomly selected spell.
        // add spell to perk name. 
        Spell spell;
        //perkName = $"Damage Increase {spell.Name}";
    }

    public override void SetDelta(float delta)
    {
        this.delta = delta;
    }

    public override void ApplyModifier(Ability ability)
    {
        Spell spellDelta = (Spell)ability;
        int deltaAsInt = (int)delta;
        float damage = spellDelta.Damage;

        if (damage > 0)
        {
            //damage *= delta;
            // multiply then add to result. that's where if should be. 
            // damage is int. so casting int it just multiples by zero. 
        }
        else if (damage < 0)
        {
            //float Mathf.Abs(damage *= delta);
        }
        spellDelta.SetDamage(deltaAsInt);
    }

    public override void RemoveModifier(Ability ability)
    {
        Spell spellDelta = (Spell)ability;
        int deltaAsInt = (int)delta;
        spellDelta.SetDamage(-deltaAsInt);
    }

    public override string GetPerkDeltaRounded()
    {
        float formatted = delta;
        string sign = formatted > 0 ? "+" : ""; // NO NEED TO PUT THE MINUS SINCE IT'S ALREADY IN THE FLOAT
        formatted *= 100f;
        string formattedText = $"{sign}{formatted:F2}";
        return formattedText;
    }
}