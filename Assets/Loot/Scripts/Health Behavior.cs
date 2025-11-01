using UnityEngine;

public class HealthBehavior : LootBehavior
{
    private Health health;

    public override void Awake()
    {
        health = (Health)loot;
        base.Awake();
    }

    public override void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Player"))
        {
            PlayerAbilities player = collided.GetComponent<PlayerAbilities>();
            if (!player.HealthIsFull())
            {
                player.AddToHealth(health.HealAmount);
                Destroy(this.gameObject);
            }
        }
        else  // IF ENVIRONMENT, CHECK TO SEE IF THE LOOT IS ABOVE THE PLATFORM. IF SO, STOP MOVEMENT
        {
            if (base.ShouldLootStopMovement(base.lootCollider, collided))
            {
                StopRigidbodyMovement();
            }
        }
    }
}