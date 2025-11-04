using System.Collections.Generic;
using UnityEngine;


public class SkillTree : MonoBehaviour {
    [SerializeField] private Ability ability;
    [SerializeField] private int skillPoints;
    [SerializeField] List<SkillNode> allNodes;
    private SkillTreeUIController uiController;
    private readonly HashSet<SkillNode> ownedUpgrades = new();
    private bool isActive;

    public int SkillPoints => skillPoints;
    public Ability AbilityToUpgrade => ability;
    public bool IsActive { get => isActive; set => isActive = value; }

    private void Awake() {
        uiController = GetComponentInParent<SkillTreeUIController>();
        isActive = false;
    }

    // CHECK IF UPGRADE IS ALREADY OWNED
    public bool UpgradeOwned(SkillNode upgrade) {
        return ownedUpgrades.Contains(upgrade);
    }

    // CHECK IF ELIGIBLE TO UPGRADE
    public DoubleBool CanUpgrade(SkillNode upgrade) {
        return upgrade.CanUpgrade(ownedUpgrades, skillPoints);
    }

    // PERFORM THE UPGRADE
    public void ApplyUpgrade(SkillNode upgrade) {
        // IF SHOULD NOT UPGRADE, RETURN
        if (UpgradeOwned(upgrade) || !CanUpgrade(upgrade).MeetsAllRequirements()) {
            return;
        }

        // UPGRADE
        ownedUpgrades.Add(upgrade);
        upgrade.ApplyUpgrade(ability);
        skillPoints -= upgrade.UpgradeCost;

        // UPDATE BUTTON TEXT AND INTERACTABILITY
        foreach (var button in FindObjectsByType<SkillUpgradeButton>(FindObjectsSortMode.None)) {
            button.UpdateButtonState();
        }

        uiController.UpdateSkillPoints();
    }

    public void SkillPointEarned() {
        skillPoints++;

        // UPDATE BUTTON TEXT AND INTERACTABILITY
        foreach (var button in FindObjectsByType<SkillUpgradeButton>(FindObjectsSortMode.None)) {
            button.UpdateButtonState();
        }
    }

    #region Save System
    public List<string> GetUnlockedUpgradeNames() {
        List<string> names = new();
        foreach (SkillNode upgrade in ownedUpgrades) {
            names.Add(upgrade.UpgradeName);
        }
        return names;
    }

    public void LoadFromSave(SkillTreeSaveData data) {
        // Ensure UI controller reference exists
        if (uiController == null)
            uiController = GetComponentInParent<SkillTreeUIController>();

        skillPoints = data.skillPoints;
        ownedUpgrades.Clear();

        foreach (string name in data.unlockedUpgrades) {
            SkillNode upgradeNode = allNodes.Find(node => node.UpgradeName == name);

            if (upgradeNode != null) {
                ownedUpgrades.Add(upgradeNode);
                upgradeNode.ApplyUpgrade(ability);
            }
            else {
                Debug.LogWarning($"[SkillTree] Could not find node named '{name}' in {this.name}");
            }
        }

        uiController?.UpdateSkillPoints();
    }
    #endregion

}