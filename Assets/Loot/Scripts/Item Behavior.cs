using System.Collections;
using UnityEngine;

public class ItemBehavior : LootBehavior
{
    private Item item;
    [SerializeField] private GameObject stationaryEffect;
    [SerializeField] private GameObject lootIconPopup;
    [SerializeField] private GameObject lootMenuPopup;
    private Upgrade[] perks;
    private bool isLootMenuActive = true;

    public override void Awake()
    {
        item = (Item)loot;
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
        if (Input.GetKeyDown(KeyCode.M)) // get interact button in input actions
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
            

            ///// move to interaction script
            //Inventory playerInventory = collided.GetComponent<Inventory>();
            //playerInventory.AddToInventory(item);
            //Destroy(this.gameObject);
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

    private void SetPerks(Upgrade[] perks)
    {
        this.perks = perks;
    }

    private IEnumerator ShowLootIconAfterDelay()
    {
        yield return new WaitForSeconds(item.IconShowDelay);
        lootIconPopup.SetActive(true);
    }    
}