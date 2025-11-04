using TMPro;
using UnityEngine;

public class SkillTreeUIController : MonoBehaviour {
    [SerializeField] GameObject[] trees;
    [SerializeField] TextMeshProUGUI[] treesTexts;
    private int activeTreeIndex = 0;
    private bool uiEnabled = false;

    //private SpellSkillTree[] spellSkillTrees;

    //private void Awake()
    //{
    //    spellSkillTrees = new SpellSkillTree[trees.Length];

    //    foreach (var tree in trees)
    //    {
    //        spellSkillTrees[activeTreeIndex] = trees[activeTreeIndex].GetComponentInParent<SpellSkillTree>();
    //    }
    //}

    public void SetUIVisibility() {
        uiEnabled = !uiEnabled;
        trees[activeTreeIndex].SetActive(uiEnabled);
        trees[activeTreeIndex].GetComponent<SkillTree>().IsActive = uiEnabled;
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

    public void UpdateSkillPoints() {
        treesTexts[activeTreeIndex].text = $"Skill Points: {trees[activeTreeIndex].GetComponent<SkillTree>().SkillPoints}";
    }
}