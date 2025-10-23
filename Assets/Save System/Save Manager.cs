using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    #region Variabls
    [Header("Component References")]
    [SerializeField] PlayerController playerController;
    [SerializeField] XPSystem xpSystem;
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] GameManager gameManager;

    [Header("Default Value Scriptable Objects")]
    [SerializeField] Player defaultPlayerAttributes;
    [SerializeField] List<Spell> defaultSpellAttributes;

    [Header("Settings")]
    [SerializeField] bool debugMode;

    #endregion

    #region I/O
    public void SaveGame() {
        SaveData data = new SaveData();

        //Meta data
        data.currentLevel = gameManager.CurrentLevel;
        data.levelSeed = levelGenerator.Seed;

        //Player data
        data.playerData = new PlayerData {
            currentHealth = playerController.Player.CurrentHealth,
            maxHealth = playerController.Player.MaxHealth,
            currentLevel = playerController.Player.CurrentLevel,
            currentXP = playerController.Player.CurrentXP
        };

        // Spell skill trees data
        data.skillTreesData = new SkillTreeCollectionSaveData();

        foreach (SkillTree tree in xpSystem.GetSkillTrees()) {
            SkillTreeSaveData treeData = new SkillTreeSaveData();
            treeData.spellName = tree.AbilityToUpgrade.name;
            treeData.skillPoints = tree.SkillPoints;
            treeData.unlockedUpgrades = tree.GetUnlockedUpgradeNames();
            data.skillTreesData.trees.Add(treeData);
        }

        SaveSystem.SaveGame(data);
    }
    public void LoadGame() {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) {
            if (debugMode) Debug.LogWarning("[SaveManager] No save file found.");
            return;
        }

        // Restore player attributes
        playerController.Player.SetMaxHealth(data.playerData.maxHealth);
        playerController.Player.SetCurrentHealth(data.playerData.currentHealth);
        playerController.Player.LoadLevelAndXP(data.playerData.currentLevel, data.playerData.currentXP);

        // Restore skill trees
        foreach (var treeData in data.skillTreesData.trees) {
            foreach (SkillTree tree in xpSystem.GetSkillTrees()) {
                if (tree.AbilityToUpgrade.name == treeData.spellName) {
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

        if (debugMode) Debug.Log("[SaveManager] Game loaded successfully.");
    }

    #endregion

    #region Utility Methods
    public void InitilalizeDefaults() {

        //Player Attributes
        LoadSOValuesFromDefault(defaultPlayerAttributes, playerController.Player);

        //Spell Attributes
        List<Spell> playerSpells = playerController.SpellBook.AllSpells.ToList();
        foreach (Spell spell in defaultSpellAttributes) {
            Spell matchingSpell = playerSpells.Find(s => s.Name == spell.Name);
            if (matchingSpell != null) {
                LoadSOValuesFromDefault(spell, matchingSpell);
            }
            else {
                if (debugMode) Debug.LogWarning($"[SaveManager] No matching spell found in SpellBook for default spell '{spell.Name}'");
            }
        }
    }

    void LoadSOValuesFromDefault(ScriptableObject defaultSO, ScriptableObject targetSO) {
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(defaultSO), targetSO);
    }
    #endregion
}

