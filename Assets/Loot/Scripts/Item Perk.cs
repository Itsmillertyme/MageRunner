using UnityEngine;

public abstract class ItemPerk : ScriptableObject
{
    [SerializeField] protected Player player;
    //[SerializeField] protected Spell spell;
    //[SerializeField] protected SpellBook spellBook;

    public abstract void OnEnable();
    public abstract void Add(float amount, int sign);
    public abstract void Remove();
}