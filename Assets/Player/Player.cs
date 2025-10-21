using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player")]

public class Player : Ability
{
    [Header("Controller")]
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float dodgeForce;
    [Range(-0.1f, -20f)]
    [SerializeField] private float gravity;

    [Header("Inventory")]
    [SerializeField] private int inventoryCapacity; // MAX ITEMS THAT CAN BE HELD

    [Header("Stats")]
    // HEALTH
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int healthRegenAmount;
    [SerializeField] private float healthRegenFrequency;
    private int healthRegenLimit;
    // STAMINA
    private int currentStamina;
    [SerializeField] private int maxStamina;
    [SerializeField] private int staminaRegenAmount;
    [SerializeField] private float staminaRegenFrequency;
    // MISC
    private float damageResistance = 0;
    [SerializeField] private float meleeDamage;
    private float healingEfficiency = 0;
    private float lootDropLuck = 0;

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
    public int HealthRegenCap => healthRegenLimit;
    public int CurrentStamina => currentStamina;
    public int MaxStamina => maxStamina;
    public int StaminaRegenAmount => staminaRegenAmount;
    public float StaminaRegenFrequency => staminaRegenFrequency;
    public float DamageResistance => damageResistance;
    public float MeleeDamage => meleeDamage;
    public float HealingEfficiency => healingEfficiency;
    public float LootDropLuck => lootDropLuck;
    public GameEvent PlayerHasDied => playerHasDied;
    public GameEvent SkillTreeMenuButtonPressed => skillTreeMenuButtonPressed;
    public AudioClip PlayerDeathSFX => playerDeathSFX;

    private void OnEnable()
    {
        healthRegenLimit = maxHealth / 2;
        currentStamina = maxStamina;
    }

    public void SaveData(Object data)
    {
        // READY FOR YA BIG DAWG
    }

    public void LoadData(Object data)
    {
        // READY FOR YA BIG DAWG
    }

    public void SetGravity(float gravity)
    {
        this.gravity = gravity;
    }

    public void IncreaseInventoryCapacity() => inventoryCapacity++;

    public void Heal(int value)
    {
        int adjustedValue = value + (int)healingEfficiency * value;
        currentHealth += adjustedValue;
    }

    public void TakeDamage(int value)
    {
        int adjustedValue = value - (int)damageResistance * value;
        currentHealth -= adjustedValue;
    }

    public void SetMaxHealth(int value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
        healthRegenLimit = maxHealth / 2;
    }

    public void ReduceCurrentStamina(int value) => currentStamina -= value;
    public void IncreaseCurrentStamina(int value) => currentStamina += value;
    public void SetMaxStamina(int value) => maxStamina += value;
    public void SetDamageResistance(float value) => damageResistance += value;
    public void SetMeleeDamage(float value) => meleeDamage += value;
    public void SetHealingEfficiency(float value) => healingEfficiency += value;
    public void SetLootDropLuck(float value) => lootDropLuck += value;
}