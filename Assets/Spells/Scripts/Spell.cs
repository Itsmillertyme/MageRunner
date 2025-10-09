using UnityEngine;

public abstract class Spell : ScriptableObject
{
    [Header("Metadata")]
    [SerializeField] private new string name;
    [SerializeField] private string description; // UNUSED // NO GETTER
    [SerializeField] private string loreText; // UNUSED // NO GETTER

    [Header("Casting")]
    [SerializeField] private int currentMana;
    [SerializeField] private int maxMana;
    [SerializeField] private int damage;
    [SerializeField] private float lifeSpan;
    [Tooltip("Delay from click to end of animation to end cast. It's divided by 30 in playercontroller")]
    [SerializeField] private float castDelayTime;
    [Tooltip("Time between casts")]
    [SerializeField] private float castCooldownTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 projectileSize;
    [SerializeField] private bool destroyOnEnemyImpact;
    [SerializeField] private bool destroyOnEnvironmentImpact;
    [SerializeField] private bool damageOverTime;
    [SerializeField] private bool canMoveDuringCast;
    [SerializeField] private bool canJumpDuringCast;

    [Header("Leveling")]
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private int currentXP;
    [SerializeField] private int baseLevelValue;
    [SerializeField] private float levelingScalar;
    private int xpToLevelUp;
    private int[] levelRequirements;


    /*
     * lvl 0 req 0 -> to lvl 1
     * lvl 1 req 1 -> to lvl 2
     * .......................
     * lvl 49 req 49 -> to lvl 50
     * 
     */


    [Header("Prefab")]
    [SerializeField] private GameObject projectile;

    [Header("SFX")]
    [SerializeField] private AudioClip spawnSFX;
    [Range(0f, 1f)]
    [SerializeField] private float spawnSFXVolume;
    [Range(-3f, 3f)]
    [SerializeField] private float spawnSFXPitch;
    [SerializeField] private AudioClip hitSFX;
    [Range(0f, 1f)]
    [SerializeField] private float hitSFXVolume;
    [Range(-3f, 3f)]
    [SerializeField] private float hitSFXPitch;
    [SerializeField] private GameObject hitSFXPrefab;

    [Header("Animation")]
    [SerializeField] private AnimationClip castAnimation;

    [Header("UI")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite reticle;

    [Header("Unlock Status")]
    [SerializeField] private bool isUnlocked;

    #region GETTERS
    public string Name => name;
    public int CurrentMana => currentMana;
    public int MaxMana => maxMana;
    public int Damage => damage;
    public float LifeSpan => lifeSpan;
    public float CastDelayTime => castDelayTime;
    public float CastCooldownTime => castCooldownTime;
    public float MoveSpeed => moveSpeed;
    public Vector3 ProjectileSize => projectileSize;
    public bool DestroyOnEnemyImpact => destroyOnEnemyImpact;
    public bool DestroyOnEnvironmentImpact => destroyOnEnvironmentImpact;
    public bool DamageOverTime => damageOverTime;
    public GameObject Projectile => projectile;
    public AudioClip SpawnSFX => spawnSFX;
    public float SpawnSFXVolume => spawnSFXVolume;
    public float SpawnSFXPitch => spawnSFXPitch;
    public AudioClip HitSFX => hitSFX;
    public float HitSFXPitch => hitSFXPitch;
    public float HitSFXVolume => hitSFXVolume;
    public GameObject HitSFXPrefab => hitSFXPrefab;
    public AnimationClip CastAnimation => castAnimation;
    public Sprite SpellIcon => icon;
    public Sprite Reticle => reticle;
    public bool IsUnlocked => isUnlocked;
    public bool CanMoveDuringCast => canMoveDuringCast;
    public bool CanJumpDuringCast => canJumpDuringCast;
    public int CurrentLevel => currentLevel;
    public int MaxLevel => maxLevel;
    public int CurrentXP => currentXP;
    public int XPToLevelUp => xpToLevelUp;
    #endregion

    public void SetLevelingData()
    {
        levelRequirements = new int[maxLevel];
        levelRequirements[0] = baseLevelValue;
        xpToLevelUp = levelRequirements[0];

        for (int i = 1; i < levelRequirements.Length; i++)
        {
            levelRequirements[i] = (int)(levelRequirements[i - 1] * levelingScalar);
        }
    }

    public void SaveData(Object data)
    {
        // READY FOR YA BIG DAWG
    }

    public void LoadData(Object data)
    {
        // READY FOR YA BIG DAWG
    }

    public void SetProjectileSize(Vector3 newValue) => projectileSize = newValue;
    public void SetMoveSpeed(float newValue) => moveSpeed = newValue;
    public void SetCastCooldownTime(float newValue) => castCooldownTime = newValue;
    public void SetDamage(int newValue) => damage = newValue;
    public void SetDestroyOnEnemyImpact(bool newValue) => destroyOnEnemyImpact = newValue;
    public void SetDestroyOnEnvironmentalImpact(bool newValue) => destroyOnEnvironmentImpact = newValue;
    public void SetDamageOverTime(bool value) => damageOverTime = value;
    public void ManaExpended() => currentMana--;

    public void SetMaxMana(int newValue)
    {
        maxMana = newValue;
        currentMana = maxMana;
    }

    public void AddToXP(int newValue) => currentXP += newValue;
    public void LeveledUp() => currentLevel++;

    public void SetNextLevelUpRequirements()
    {
        if (currentLevel == maxLevel) return;

        xpToLevelUp = levelRequirements[currentLevel];
    }
}