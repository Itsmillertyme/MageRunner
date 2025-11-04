using UnityEngine;

public class HealthBehavior : LootBehavior
{
    private Health health;
    [Tooltip("THE AMOUNT OF HEALTH THAT WILL BE DROPPED")]
    private int replenishAmount;
    private Vector3 baseScale;

    public override void Awake()
    {
        health = (Health)loot;
        baseScale = transform.localScale;
        SetDropCountAndScale();
        base.Awake();
    }

    public override void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Player"))
        {
            PlayerAbilities player = collided.GetComponent<PlayerAbilities>();
            player.AddToHealth(replenishAmount);
            Destroy(this.gameObject);
        }
        else  // IF ENVIRONMENT, CHECK TO SEE IF THE LOOT IS ABOVE THE PLATFORM. IF SO, STOP MOVEMENT
        {
            if (base.ShouldLootStopMovement(base.lootCollider, collided))
            {
                base.StopRigidbodyMovement();
            }
        }
    }

    private void SetDropCountAndScale()
    {
        replenishAmount = health.MaxHealthDropped + UtilityTools.RandomVarianceInt(-health.MaxHealthDropped + 1, 0);
        float ratio = (float)replenishAmount / (float)health.MaxHealthDropped;
        float scale = baseScale.x * ratio;
        float finalScale = Mathf.Max(scale, health.MinLocalScale);
        transform.localScale = new(finalScale, finalScale, finalScale);
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
}