using System.Collections;
using UnityEngine;

public abstract class LootBehavior : MonoBehaviour {
    [Header("References")]
    public Loot loot;
    [SerializeField] private GameObject blinkingParent;
    private Rigidbody rb;
    [HideInInspector] public Collider lootCollider;
    private Coroutine coroutine;

    public virtual void Awake() {
        rb = GetComponent<Rigidbody>();
        lootCollider = GetComponent<Collider>();
        FlingLoot(GetPhysicsValues());
        DisappearAfterTime();
    }

    public abstract void OnTriggerEnter(Collider collided);

    private void OnDestroy() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
    }

    public virtual (Vector3, Vector3, Vector3) GetPhysicsValues() {
        // MOVEMENT UPWARD
        Vector3 up = Vector3.up * (loot.UpwardForce + UtilityTools.RandomVarianceFloat(-1f, 1f));

        // OUTWARD
        float randomX = UtilityTools.RandomVarianceFloat(-1f, 1f);
        Vector3 outward = new Vector3(randomX, 0f, 0f).normalized * loot.OutwardForce;

        // SPIN
        float torqueX = UtilityTools.RandomVarianceFloat(-loot.TorqueStrength, loot.TorqueStrength);
        float torqueY = UtilityTools.RandomVarianceFloat(-loot.TorqueStrength, loot.TorqueStrength);
        float torqueZ = UtilityTools.RandomVarianceFloat(-loot.TorqueStrength, loot.TorqueStrength);
        Vector3 spin = new(torqueX, torqueY, torqueZ);

        return (up, outward, spin);
    }

    public void FlingLoot((Vector3, Vector3, Vector3) vectors) {
        rb.AddForce(vectors.Item1 + vectors.Item2, ForceMode.Impulse);
        rb.AddTorque(vectors.Item3, ForceMode.Impulse);
    }

    public void StopRigidbodyMovement() {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public bool ShouldLootStopMovement(Collider lootCollider, Collider environmentCollider) {
        bool result = false;

        if (Physics.ComputePenetration(lootCollider, lootCollider.transform.position,
            lootCollider.transform.rotation, environmentCollider, environmentCollider.transform.position,
            environmentCollider.transform.rotation, out Vector3 direction, out _)) {
            if (direction.y >= 0.5f) // ENVIRONMENT IS ABOVE LOOT
            {
                result = true;
            }
            else // ENVIRONMENT IS BELOW LOOT
            {
                result = false;
            }
        }
        return result;
    }

    private void DisappearAfterTime() {
        Destroy(this.gameObject, loot.LifeSpan);
        coroutine = StartCoroutine(BlinkWhenCloseToDestroy());
    }

    private IEnumerator BlinkWhenCloseToDestroy() {
        //float quarterLifeRemaining = loot.LifeSpan - (loot.LifeSpan * 0.75f);
        //float blinkRepeatSpeed = quarterLifeRemaining / 4;
        //yield return new WaitForSeconds(quarterLifeRemaining);

        //while (true)
        //{
        //    blinkingParent.SetActive(false);
        //    yield return new WaitForSeconds(blinkRepeatSpeed);
        //    blinkingParent.SetActive(true);
        //}

        yield return null;
    }
}