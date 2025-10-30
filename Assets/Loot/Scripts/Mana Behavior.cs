using UnityEngine;

public class ManaBehavior : LootBehavior
{
    public override void OnTriggerEnter(Collider collided)
    {
        switch (loot)
        {
            case Mana mana:
                if (collided.CompareTag("Player")) // IF PLAYER, ADD MANA AND UPDATE UI, THEN DELETE COLLIDED
                {
                    SpellBook spellBook = collided.GetComponent<SpellBook>();
                    if (!spellBook.ManaIsFull())
                    {
                        spellBook.SetCurrentMana(mana.ManaRecoverAmount);
                        spellBook.UpdateUI();
                        Destroy(this);
                    }
                }
                else // IF ENVIRONMENT, STOP MOVEMENT
                {
                    rb.useGravity = false;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                break;
        }
    }
}