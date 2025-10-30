using UnityEngine;

public class ItemBehavior : LootBehavior
{
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

    public override void OnTriggerEnter(Collider collided)
    {
        switch (loot)
        {
            case Item item:
                // TBD I'm thinking do like a DoT type deal. tack on a script that has a kill timer. it records default values, adds to them, then ondestroy changes them back.
                // item modifier created for this. 
                break;
        }
    }
}