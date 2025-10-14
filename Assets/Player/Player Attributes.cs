using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Attributes")]

public class PlayerAttributes : ScriptableObject {
    [Header("Controller")]
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float dodgeForce;
    [Range(-0.1f, -20f)]
    [SerializeField] private float gravity;

    [Header("Health")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int healthRegenAmount;
    [SerializeField] private float healthRegenFrequency;

    [Header("Leveling")]
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    private int currentXP;
    [SerializeField] private int baseLevelValue;
    [SerializeField] private float levelingScalar;
    private int xpToLevelUp;
    private int[] levelRequirements;

    [Header("Events")]
    [SerializeField] private GameEvent playerHasDied;
    [SerializeField] private GameEvent skillTreeMenuButtonPressed;

    [Header("SFX")]
    [SerializeField] private AudioClip playerDeathSFX;

    public float MaxJumpHeight => maxJumpHeight;
    public float MaxJumpTime => maxJumpTime;
    public float MovementSpeed => movementSpeed;
    public float SprintMultiplier => sprintMultiplier;
    public float DodgeForce => dodgeForce;
    public float Gravity => gravity;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int HealthRegenAmount => healthRegenAmount;
    public float HealthRegenFrequency => healthRegenFrequency;
    public GameEvent PlayerHasDied => playerHasDied;
    public GameEvent SkillTreeMenuButtonPressed => skillTreeMenuButtonPressed;
    public AudioClip PlayerDeathSFX => playerDeathSFX;

    public void SaveData(Object data) {
        // READY FOR YA BIG DAWG
    }

    public void LoadData(Object data) {
        // READY FOR YA BIG DAWG
    }

    public void SetCurrentHealth(int value) => currentHealth += value;
    public void SetMaxHealth(int value) {
        maxHealth += value;
        currentHealth = maxHealth;
    }

    public void SetLevelingData() {
        levelRequirements = new int[maxLevel];
        levelRequirements[0] = baseLevelValue;
        xpToLevelUp = levelRequirements[0];

        for (int i = 1; i < levelRequirements.Length; i++) {
            levelRequirements[i] = (int) (levelRequirements[i - 1] * levelingScalar);
        }
    }

    public void AddToXP(int newValue) => currentXP += newValue;
    public void LeveledUp() => currentLevel++;

    public void SetNextLevelUpRequirements() {
        if (currentLevel == maxLevel) return;

        xpToLevelUp = levelRequirements[currentLevel];
    }

    public void SetGravity(float gravity)
    {
        this.gravity = gravity;
    }
}