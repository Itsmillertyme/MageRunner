using UnityEngine;

public abstract class SkillUpgrade : ScriptableObject
{
    public abstract void Apply(Spell spell);
}