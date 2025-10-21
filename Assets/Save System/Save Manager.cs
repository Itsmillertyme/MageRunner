using UnityEngine;

public class SaveManager : MonoBehaviour {
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] XPSystem xpSystem;
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] GameManager gameManager;

    public void SaveGame() {
        SaveData data = new SaveData();

        //Meta data
        data.currentLevel = gameManager.CurrentLevel;
        data.levelSeed = levelGenerator.Seed;

        //Player data
        data.playerData = new PlayerData {
            currentHealth = playerAttributes.CurrentHealth,
            maxHealth = playerAttributes.MaxHealth,
            currentLevel = playerAttributes.CurrentLevel,
            currentXP = playerAttributes.CurrentXP
        };

        // Spell skill trees data
        data.skillTreesData = new SkillTreeCollectionSaveData();

        foreach (SkillTree tree in xpSystem.GetSkillTrees()) {
            SkillTreeSaveData treeData = new SkillTreeSaveData();
            treeData.spellName = tree.SelectedSpell.name;
            treeData.skillPoints = tree.SkillPoints;
            treeData.unlockedUpgrades = tree.GetUnlockedUpgradeNames();
            data.skillTreesData.trees.Add(treeData);
        }

        SaveSystem.SaveGame(data);
    }


    public void LoadGame() {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) {
            Debug.LogWarning("[SaveManager] No save file found.");
            return;
        }

        // Restore player attributes
        playerAttributes.SetMaxHealth(data.playerData.maxHealth);
        playerAttributes.SetCurrentHealth(data.playerData.currentHealth);
        playerAttributes.LoadLevelAndXP(data.playerData.currentLevel, data.playerData.currentXP);

        // Restore skill trees
        foreach (var treeData in data.skillTreesData.trees) {
            foreach (SkillTree tree in xpSystem.GetSkillTrees()) {
                if (tree.SelectedSpell.name == treeData.spellName) {
                    tree.LoadFromSave(new SkillTreeSaveData {
                        skillPoints = treeData.skillPoints,
                        unlockedUpgrades = treeData.unlockedUpgrades
                    });
                    break;
                }
            }
        }

        // Restore level
        levelGenerator.Seed = data.levelSeed;
        levelGenerator.GenerateLevel();

        Debug.Log("[SaveManager] Game loaded successfully.");
    }
}
