using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeTooltipController : MonoBehaviour {
    #region Variables
    [Header("References")]
    [SerializeField] SkillTree skillTree;
    [SerializeField] RectTransform skillTreeCanvasRect;
    [SerializeField] GameObject tooltip;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costText;

    public bool cursorOnNode;
    #endregion

    #region Unity Methods    
    private void Update() {
        if (skillTree.IsActive) {
            if (cursorOnNode) {
                tooltip.SetActive(true);

                //Make tooltip follow mouse
                RectTransform rect = (RectTransform) transform;
                Vector2 mousePos = Input.mousePosition;
                Vector2 localPoint;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(skillTreeCanvasRect, mousePos, null, out localPoint);

                localPoint.y -= 60f;
                rect.anchoredPosition = localPoint;
            }
            else {
                tooltip.SetActive(false);
            }
        }
    }
    #endregion

    #region Utility Methods

    public void OnPointerEnter(BaseEventData eventData) {
        PointerEventData pointerData = eventData as PointerEventData;

        if (pointerData != null) {
            GameObject nodeIcon = pointerData.pointerEnter;
            SkillNode node = nodeIcon.GetComponent<SkillUpgradeButton>().UpgradeNode;
            descriptionText.text = node.UpgradeName;
            costText.text = $"Cost: {node.UpgradeCost}";
        }

        cursorOnNode = true;
    }

    public void OnPointerExit(BaseEventData eventData) {
        cursorOnNode = false;
    }
    #endregion
}
