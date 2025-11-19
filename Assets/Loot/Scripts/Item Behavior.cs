using System.Collections;
using UnityEngine;

public class ItemBehavior : LootBehavior
{
    private Item item;
    [SerializeField] private GameObject stationaryEffect;
    [SerializeField] private GameObject lootIconPopup;
    [SerializeField] private GameObject lootMenuPopup;
    private bool isPlayerInRange = false;
    private bool isLootMenuActive = false;

    public override void Awake() // BASE AWAKE GETS RB AND COLLIDER. THEN FLINGS LOOT AND SETS DISAPPEAR AFTER TIME. 
    {
        item = Instantiate(loot as Item);
        item.ChooseRandomPerks();   
        base.Awake();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.M)) // GET IUNTERACT BUTTON INPUT HERE
        {
            isLootMenuActive = !isLootMenuActive;
            lootIconPopup.SetActive(!isLootMenuActive);
            lootMenuPopup.SetActive(isLootMenuActive);
        }
    }

    public override void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Player"))
        {
            isPlayerInRange = true; // SET FLAG FOR IS IN RANGE
            lootIconPopup.SetActive(true);
            return;
        }

        if (base.ShouldLootStopMovement(base.lootCollider, collided)) // IF ENVIRONMENT, CHECK TO SEE IF THE LOOT IS ABOVE THE PLATFORM. IF SO, STOP MOVEMENT 
        {
            // STOP MOVEMENT
            base.StopRigidbodyMovement();

            // ENABLE STATIONARY VFX AND INTERACT ICON
            stationaryEffect.SetActive(true);
            StartCoroutine(ShowLootIconAfterDelay());
        }
    }

    private void OnTriggerExit(Collider collided)
    {
        if (collided.CompareTag("Player"))
        {
            isPlayerInRange = false; // SET FLAG FOR IS OUT OF RANGE

            if (lootMenuPopup.activeSelf) // CLOSE MENU IF OPEN AND EXITING
            {
                lootIconPopup.SetActive(true);
                lootMenuPopup.SetActive(false);
                isLootMenuActive = true;
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

    private IEnumerator ShowLootIconAfterDelay()
    {
        yield return new WaitForSeconds(item.IconShowDelay);
        lootIconPopup.SetActive(true);
    }

    public ItemInventoryData GetItemInventoryData()
    {
        ItemInventoryData data = new(item.Rarity, item.ItemIcon, item.Perks, item.PerksDeltas, item.ItemName);
        return data;
    }
}