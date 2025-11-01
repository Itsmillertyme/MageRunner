using UnityEngine;

public class ItemBehavior : LootBehavior
{
    private Item item;

    public override void Awake()
    {
        item = (Item)loot;
        base.Awake();
    }

    public override void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Player"))
        {

        }
        else  // IF ENVIRONMENT, CHECK TO SEE IF THE LOOT IS ABOVE THE PLATFORM. IF SO, STOP MOVEMENT
        {
            if (base.ShouldLootStopMovement(base.lootCollider, collided))
            {
                StopRigidbodyMovement();
            }
        }
    }

    public override (Vector3, Vector3, Vector3) GetPhysicsValues()
    {
        // MOVEMENT UPWARD
        Vector3 up = Vector3.up * (loot.UpwardForce + UtilityTools.RandomVarianceFloat(-1f, 1f));

        // OUTWARD
        float randomX = UtilityTools.RandomVarianceFloat(-1f, 1f);
        Vector3 outward = new Vector3(randomX, 0f, 0f).normalized * loot.OutwardForce;

        // SPIN
        Vector3 spin = Vector3.zero;

        return (up, outward, spin);
    }

    // TURN THE VFX OFF UNTIL STOPPED FOR THE GROUND VFX. MIGHT NEED TO LIFT THE COLLIDER A BIT TOO
}