using UnityEngine;

public abstract class ItemPerk : ScriptableObject
{
    protected ModifyAbility abilityDelta;

    public ModifyAbility AbilityDelta => abilityDelta;

    public abstract void SetDelta(float amount);
    public abstract void ApplyModifier(Ability ability);
    public abstract void RemoveModifier(Ability ability);
}

public enum ModifyAbility
{
    Player,
    Spellbook
}