using System.Collections;
using UnityEngine;

public class ItemBehavior : LootBehavior
{
    private Item item;
    [SerializeField] private GameObject stationaryEffect;
    [SerializeField] private GameObject lootIconPopup;
    [SerializeField] private GameObject lootMenuPopup;
    [SerializeField] private ItemPerk[] perks;
    [SerializeField] private float[] perksDeltas;
    private bool isLootMenuActive = true;

    public override void Awake()
    {
        item = (Item)loot;
        SetPerks();
        base.Awake();
    }

    public override void OnTriggerEnter(Collider collided)
    {
        if (!collided.CompareTag("Player")) // IF ENVIRONMENT, CHECK TO SEE IF THE LOOT IS ABOVE THE PLATFORM. IF SO, STOP MOVEMENT 
        {
            if (base.ShouldLootStopMovement(base.lootCollider, collided))
            {
                base.StopRigidbodyMovement();

                // ENABLE STATIONARY VFX
                stationaryEffect.SetActive(true);
                StartCoroutine(ShowLootIconAfterDelay());
            }
        }
    }

    private void OnTriggerStay(Collider collided)
    {
        if (collided.CompareTag("Player") && Input.GetKeyDown(KeyCode.M)) // get interact button in input actions
        {
            isLootMenuActive = !isLootMenuActive;
            if (isLootMenuActive)
            {
                lootIconPopup.SetActive(true);
                lootMenuPopup.SetActive(false);
            }
            else
            {
                lootIconPopup.SetActive(false);
                lootMenuPopup.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider collided)
    {
        if (lootMenuPopup.activeSelf)
        {
            lootIconPopup.SetActive(true);
            lootMenuPopup.SetActive(false);
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

    private void SetPerks()
    {
        (ItemPerk[], float[]) perkInfo = item.SetPerks();
        perks = perkInfo.Item1;
        perksDeltas = perkInfo.Item2;

        // APPLY PERKS
        string debugmsg = "";
        for (int i = 0; i < perks.Length; i++)
        {
            int sign = UtilityTools.RandomVarianceInt(0, 1);  // SET SIGN VALUE. 0 FOR POSITIVE, 1 FOR NEGATIVE.

            if (sign == 1) // MAKE AMOUNT NEGATIVE
            {
                perksDeltas[i] *= -1;
            }

            perks[i].Set(perksDeltas[i]);
            debugmsg += $"{perks[i]} perk with value {perksDeltas[i]}\n";
        }
        DeveloperScript.Instance.debug(debugmsg, true);
    }

    private IEnumerator ShowLootIconAfterDelay()
    {
        yield return new WaitForSeconds(item.IconShowDelay);
        lootIconPopup.SetActive(true);
    }

    public ItemInventoryData GetItemInventoryData()
    {
        ItemInventoryData data = new(item.Rarity, item.ItemIcon, perks, perksDeltas, item.ItemName);
        return data;
    }
}