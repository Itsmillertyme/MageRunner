using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeUIController : MonoBehaviour {
    #region Variables
    [Header("References")]
    [SerializeField] GameObject[] trees;
    [SerializeField] TextMeshProUGUI[] treesTexts;
    [SerializeField] GameObject tabs;

    private int activeTreeIndex = 0;
    private bool uiEnabled = false;
    #endregion

    #region Unity Methods
    private void Awake() {
        tabs.SetActive(false);
    }
    #endregion

    #region Utility Methods
    public void SetUIVisibility() {
        uiEnabled = !uiEnabled;
        trees[activeTreeIndex].SetActive(uiEnabled);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = uiEnabled;
        tabs.SetActive(uiEnabled);
        UpdateSkillPoints();
    }

    public void NextTree() {
        trees[activeTreeIndex].SetActive(false);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = false;

        if (activeTreeIndex < trees.Length - 1) {
            activeTreeIndex++;
        }
        else {
            activeTreeIndex = 0;
        }

        trees[activeTreeIndex].SetActive(true);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = true;
        UpdateSkillPoints();
    }

    public void PreviousTree() {
        trees[activeTreeIndex].SetActive(false);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = false;

        if (activeTreeIndex > 0) {
            activeTreeIndex--;
        }
        else {
            activeTreeIndex = trees.Length - 1;
        }

        trees[activeTreeIndex].SetActive(true);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = true;
        UpdateSkillPoints();
    }

    public void SelectTree(int index) {
        trees[activeTreeIndex].SetActive(false);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = false;

        activeTreeIndex = index;

        trees[activeTreeIndex].SetActive(true);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = true;
    }

    public void SetTreeFromTab(BaseEventData eventData) {
        //Set up event data
        PointerEventData pointerData = eventData as PointerEventData;
        GameObject clickedButton = pointerData.pointerPress != null ? pointerData.pointerPress : pointerData.pointerEnter;

        SelectTree(clickedButton.transform.GetSiblingIndex());
    }

    public void UpdateSkillPoints() {
        treesTexts[activeTreeIndex].text = $"Skill Points: {trees[activeTreeIndex].GetComponent<SkillTree>().SkillPoints}";
    }

    #endregion
}