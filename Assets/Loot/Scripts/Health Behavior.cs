using UnityEngine;

public class HealthBehavior : LootBehavior
{
    public override void OnTriggerEnter(Collider collided)
    {
        switch (loot)
        {
            case Health health:
                PlayerAbilities player = collided.GetComponent<PlayerAbilities>();
                if (!player.HealthIsFull())
                {
                    player.AddToHealth(health.HealAmount);
                    Destroy(this.gameObject);
                }
                break;
        }
    }
}