using UnityEngine;

public abstract class ItemPerk : ScriptableObject
{
    public abstract void SetDelta(float amount);
    public abstract void ApplyModifier(Ability ability);
    public abstract void RemoveModifier(Ability ability);
}