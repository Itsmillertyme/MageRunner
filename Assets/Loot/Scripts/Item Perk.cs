using UnityEngine;

public abstract class ItemPerk : ScriptableObject
{
    protected ModifyAbility abilityDelta;
    protected string perkName = "";

    public ModifyAbility AbilityDelta => abilityDelta;
    public string ItemName => perkName;

    public abstract void SetDelta(float amount);
    public abstract void ApplyModifier(Ability ability);
    public abstract void RemoveModifier(Ability ability);
    public abstract string GetPerkDeltaRounded();
}

public enum ModifyAbility
{
    Player,
    Spellbook
}