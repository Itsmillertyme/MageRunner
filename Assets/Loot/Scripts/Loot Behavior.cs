using System.Collections;
using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Loot loot;

    private Rigidbody rb;
    private Coroutine coroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        FlingLoot(GetPhysicsValues());
        DisappearAfterTime();
    }

    private (Vector3, Vector3, Vector3) GetPhysicsValues()
    {
        // MOVEMENT
        // UPWARD
        Vector3 up = Vector3.up * loot.UpwardForce;

        // OUTWARD
        float randomX = UtilityTools.RandomVarianceFloat(1f);
        Vector3 outward = new Vector3(randomX, 0f, 0f).normalized * loot.OutwardForce;

        // SPIN
        float torqueX = UtilityTools.RandomVarianceFloat(loot.TorqueStrength);
        float torqueY = UtilityTools.RandomVarianceFloat(loot.TorqueStrength);
        float torqueZ = UtilityTools.RandomVarianceFloat(loot.TorqueStrength);
        Vector3 spin = new(torqueX, torqueY, torqueZ);

        return (up, outward, spin);
    }

    public void FlingLoot((Vector3, Vector3, Vector3) vectors)
    {
        rb.AddForce(vectors.Item1 + vectors.Item2, ForceMode.Impulse);
        rb.AddTorque(vectors.Item3, ForceMode.Impulse);
    }

    private void DisappearAfterTime()
    {
        Destroy(this.gameObject, loot.LifeSpan);
        coroutine = StartCoroutine(BlinkWhenCloseToDestroy());
    }

    private IEnumerator BlinkWhenCloseToDestroy()
    {
        float quarterLifeRemaining = loot.LifeSpan * 0.75f;
        float blinkRepeatSpeed = quarterLifeRemaining / 10f;
        yield return new WaitForSeconds(quarterLifeRemaining);

        while (true)
        {
            yield return new WaitForSeconds(blinkRepeatSpeed);
        }
    }

    private void OnTriggerEnter(Collider collided)
    {
        // if loot is mana and player active spell mana is not full, pickup
        // otherwise add item
        if (!collided.CompareTag("Player")) // temp while debugging. 
        {
            //Debug.Log(collided.name);
            return;
        }

        switch (loot)
        {
            case Item item:
                // TBD I'm thinking do like a DoT type deal. tack on a script that has a kill timer. it records default values, adds to them, then ondestroy changes them back.
                // item modifier created for this. 
                break;
            case Health health:
                PlayerAbilities player = collided.GetComponent<PlayerAbilities>();
                if (!player.HealthIsFull())
                {
                    player.AddToHealth(health.HealAmount);
                    Destroy(this.gameObject);
                }
                break;
            case Mana mana:
                SpellBook spellBook = collided.GetComponent<SpellBook>();
                if (!spellBook.ManaIsFull())
                {
                    spellBook.SetCurrentMana(mana.ManaRecoverAmount);
                    spellBook.UpdateUI();
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}