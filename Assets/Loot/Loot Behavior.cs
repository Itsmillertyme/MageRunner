using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    [Header("Forces On Loot Spawn")]
    [SerializeField] private float upwardForce;
    [SerializeField] private float outwardForce;
    [SerializeField] private float torqueStrength;

    [Header("References")]
    [SerializeField] private Loot loot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        FlingLoot(GetPhysicsValues());
    }

    private (Vector3, Vector3, Vector3) GetPhysicsValues()
    {
        // MOVEMENT
        // UPWARD
        Vector3 up = Vector3.up * upwardForce;

        // OUTWARD
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 outward = new Vector3(randomX, 0f, randomZ).normalized * outwardForce;

        // SPIN
        float torqueX = Random.Range(-torqueStrength, torqueStrength);
        float torqueY = Random.Range(-torqueStrength, torqueStrength);
        float torqueZ = Random.Range(-torqueStrength, torqueStrength);
        Vector3 spin = new(torqueX, torqueY, torqueZ);

        return (up, outward, spin);
    }

    public void FlingLoot((Vector3, Vector3, Vector3) vectors)
    {
        rb.AddForce(vectors.Item1 + vectors.Item2, ForceMode.Impulse);
        rb.AddTorque(vectors.Item3, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider collided)
    {
        // if loot is mana and player active spell mana is not full, pickup
        // otherwise add item

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
                }
                break;
            case Mana mana:
                SpellBook spellBook = collided.GetComponent<SpellBook>();
                if (!spellBook.ManaIsFull())
                {
                    spellBook.SetCurrentMana(mana.ManaRecoverAmount);
                }
                break;
        }
    }
}