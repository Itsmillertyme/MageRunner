using UnityEngine;

public abstract class ItemPerk : ScriptableObject
{
    protected Player player;
    protected SpellBook spellBook;
    //protected Spell spell;

    public virtual void OnEnable()
    {
        player = FindFirstObjectByType<PlayerAbilities>().PlayerSO;
        spellBook = FindFirstObjectByType<SpellBook>();
    }

    public abstract void Set(float amount);
    public abstract void Apply();
    public abstract void Remove();
}