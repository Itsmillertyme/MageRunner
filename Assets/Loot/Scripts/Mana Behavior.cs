using System.Collections;
using UnityEngine;

public class ManaBehavior : LootBehavior
{
    private Mana mana;
    private SpellBook spellBook;
    private Transform player;
    private int manaDrop; // THE AMOUNT OF MANA THAT WILL BE DROPPED
    private bool canPursue = false;

    public override void Awake()
    {
        mana = (Mana)loot;
        spellBook = FindFirstObjectByType<SpellBook>();
        player = FindFirstObjectByType<PlayerAbilities>().gameObject.transform;
        SetDrop();
        StartCoroutine(DelayPursuit());
        base.Awake();
    }

    private void Update()
    {
        PursuePlayer();
    }

    public override void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Player")) // IF PLAYER, ADD MANA AND UPDATE UI, THEN DELETE COLLIDED
        {
            spellBook.SetSpecificSpellMana(mana.SpellToReplenish, manaDrop);
            spellBook.UpdateUI();
            Destroy(this.gameObject);
        }
        else // IF ENVIRONMENT, CHECK TO SEE IF THE LOOT IS ABOVE THE PLATFORM. IF SO, STOP MOVEMENT
        {
            if (base.ShouldLootStopMovement(base.lootCollider, collided))
            {
                base.StopRigidbodyMovement();
            }
        }
    }

    private void SetDrop()
    {
        manaDrop = mana.MaxManaDropped + UtilityTools.RandomVarianceInt(-mana.MaxManaDropped + 1, 0);
        float ratio = (float)manaDrop / (float)mana.MaxManaDropped;
        float scale = Mathf.Max(ratio, mana.MinManaScale);
        transform.localScale = new(scale, scale, scale);
    }

    private void PursuePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= mana.PursuitRange && canPursue)
        {
            base.StopRigidbodyMovement();
            transform.LookAt(player.position);
            float distanceDelta = mana.PursuitSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, distanceDelta);
        }
    }

    private IEnumerator DelayPursuit()
    {
        yield return new WaitForSeconds(mana.PursuitDelay);
        canPursue = true;
    }
}