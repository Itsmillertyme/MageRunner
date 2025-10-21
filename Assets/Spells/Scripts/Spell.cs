using UnityEngine;

public abstract class Spell : Ability
{
    [Header("Metadata")]
    [SerializeField] private new string name;
    //[SerializeField] private string description;
    //[SerializeField] private string loreText;

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
    public Sprite Icon => icon;
    public Sprite Reticle => reticle;
    public bool IsUnlocked => isUnlocked;
    public bool CanMoveDuringCast => canMoveDuringCast;
    public bool CanJumpDuringCast => canJumpDuringCast;

    public void SaveData(GameObject data)
    {
        // READY FOR YA BIG DAWG
    }

    public void LoadData(GameObject data)
    {
        // READY FOR YA BIG DAWG
    }

    public void SetProjectileSize(Vector3 value) => projectileSize = value;
    public void SetMoveSpeed(float value) => moveSpeed = value;
    public void SetCastCooldownTime(float value) => castCooldownTime = value;
    public void SetDamage(int value) => damage = value;
    public void SetDestroyOnEnemyImpact(bool value) => destroyOnEnemyImpact = value;
    public void SetDestroyOnEnvironmentalImpact(bool value) => destroyOnEnvironmentImpact = value;
    public void SetDamageOverTime(bool value) => damageOverTime = value;
    public void ManaExpended() => currentMana--;
    public void SetCurrentMana(int addition)
    {
        int manaDeficit = maxMana - currentMana;

        if (addition <= manaDeficit)
        {
            currentMana += addition;
        }
        else
        {
            currentMana = maxMana;
        }
    }

    public void SetMaxMana(int value)
    {
        maxMana += value;
        currentMana = maxMana;
    }
}