using System.Collections;
using UnityEngine;

public abstract class LootBehavior : MonoBehaviour
{
    [Header("References")]
    public Loot loot;
    [SerializeField] private GameObject blinkingParent;
    [HideInInspector] public Rigidbody rb;
    private Coroutine coroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        FlingLoot(GetPhysicsValues());
        DisappearAfterTime();
    }

    public abstract void OnTriggerEnter(Collider collided);

    public virtual (Vector3, Vector3, Vector3) GetPhysicsValues()
    {
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

    private void OnDestroy()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}