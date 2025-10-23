using UnityEngine;

public class Ability : ScriptableObject {
    [Header("Leveling")]
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private int currentXP;
    [SerializeField] private int baseLevelValue;
    [SerializeField] private float levelingScalar;
    private int xpToLevelUp;
    private int[] levelRequirements;

    public int CurrentLevel => currentLevel;
    public int MaxLevel => maxLevel;
    public int CurrentXP => currentXP;
    public int XPToLevelUp => xpToLevelUp;

    public void SetLevelingData() {
        levelRequirements = new int[maxLevel];
        levelRequirements[0] = baseLevelValue;
        xpToLevelUp = levelRequirements[0];

        for (int i = 1; i < levelRequirements.Length; i++) {
            levelRequirements[i] = (int) (levelRequirements[i - 1] * levelingScalar);
        }
    }

    public void AddToXP(int value) => currentXP += value;
    public void LeveledUp() => currentLevel++;

    public void SetNextLevelUpRequirements() {
        if (currentLevel == maxLevel) return;

        xpToLevelUp = levelRequirements[currentLevel];
    }

    #region Save System

    public void LoadLevelAndXP(int level, int xp) {
        currentLevel = level;
        currentXP = xp;
    }


    #endregion
}