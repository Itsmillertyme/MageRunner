using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour {
    #region Variables
    [Header("UI Settings")]
    [SerializeField] float playerDetectionRedius;
    [SerializeField] Color legendaryColor;
    [SerializeField] Color exoticColor;
    [SerializeField] Color rareColor;
    [SerializeField] Color uncommonColor;
    [SerializeField] Color commonColor;

    [Header("UI References")]
    [SerializeField] RectTransform itemCanvasRect;
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] Image itemIcon;
    [SerializeField] Image decoration1;
    [SerializeField] Image decoration2;
    [SerializeField] TextMeshProUGUI buffsTMP;
    [SerializeField] TextMeshProUGUI debuffsTMP;
    //
    bool isOnRightSide;
    Transform playerTransform;
    #endregion

    #region Unity Methods
    private void FixedUpdate() {
        if (playerTransform != null) {
            //position loot UI
            bool prevUIPosition = isOnRightSide;
            isOnRightSide = playerTransform.position.x > transform.position.x; //Test if player on left

            if (prevUIPosition != isOnRightSide) {
                //swap position             
                itemCanvasRect.anchoredPosition = new Vector2(-itemCanvasRect.anchoredPosition.x, itemCanvasRect.anchoredPosition.y);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //show loot UI
            itemCanvasRect.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            //hide loot UI            
            itemCanvasRect.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Utility Methods
    public void Initialize(Rarity rarityIn, string titleIn, Sprite iconIn, string buffsIn, string debuffsIn) {
        //set statics
        titleTMP.text = titleIn;
        itemIcon.sprite = iconIn;
        buffsTMP.text = buffsIn;
        debuffsTMP.text = debuffsIn;
        isOnRightSide = true; //default

        //set decoration color
        Color itemRarityColor = GetColorBasedOnRarity(rarityIn);
        decoration1.color = itemRarityColor;
        decoration2.color = itemRarityColor;

        //find player transform
        playerTransform = GameObject.FindFirstObjectByType<PlayerAbilities>().transform;

        //set player detection
        GetComponent<SphereCollider>().radius = playerDetectionRedius;

        //hide canvas
        itemCanvasRect.gameObject.SetActive(false);
    }

    private Color GetColorBasedOnRarity(Rarity itemRarity) {
        Color color = new Color();
        switch (itemRarity) {
            case Rarity.Legendary: color = legendaryColor; break;
            case Rarity.Exotic: color = exoticColor; break;
            case Rarity.Rare: color = rareColor; break;
            case Rarity.Uncommon: color = uncommonColor; break;
            default: color = commonColor; break;
        }
        return color;
    }
    #endregion
}
