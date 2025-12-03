using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat_BossLvl1 : MonoBehaviour, IEnemyCombatBehaviour {
    #region Variables

    [Header("Crit Settings")]
    [SerializeField, Tooltip("Damage multiplier for crit melee attacks.")]
    float critMultiplier = 1.5f;

    [Header("Cooldown Multipliers")]
    [SerializeField] float meleeCooldownMultiplier = 1.0f;
    [SerializeField] float rangedCooldownMultiplier = 1.0f;

    [Header("Super Attack Settings")]
    [SerializeField, Range(0f, 1f)] float superAttackChance = 0.25f;
    [SerializeField] float superAttackCooldown = 6f;

    [Header("Projectile Overrides (Optional)")]
    [SerializeField] GameObject projectilePrefabOverride;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject weaponParticles;

    [Header("Debug (Read Only)")]
    [SerializeField] bool initialized = false;
    [SerializeField] bool playerInBossRoom = false;
    [SerializeField] bool attackReady = true;
    [SerializeField] bool superReady = true;
    [SerializeField] bool playerInMeleeRange = false;
    [SerializeField] bool playerInRangedRange = false;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private EnemyProfile profile;

    private float meleeWindupTime;
    private float meleeRecoveryTime;
    private float rangedWindupTime;
    private float rangedRecoveryTime;

    private float meleeCooldown;
    private float rangedCooldown;

    private int meleeDamage;
    private int rangedDamage;

    private float meleeMin;
    private float meleeMax;
    private float meleeIdeal;

    private float chargeRange;
    private float attackRange;

    private GameObject projectilePrefab;
    private float projectileSpeed;
    private float projectileLifetime;

    public bool PlayerInBossRoom { get => playerInBossRoom; set => playerInBossRoom = value; }

    #endregion

    #region Interface Methods

    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        EnemyBrain brain = GetComponent<EnemyBrain>();
        profile = brain != null ? brain.Profile : null;

        if (profile == null) {
            Debug.LogError($"{name} Boss Combat: No EnemyProfile assigned!");
            return;
        }

        // Player reference
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;

        // Radii assignments
        meleeMin = profile.attackMinRange;
        meleeIdeal = profile.attackIdealRange;
        meleeMax = profile.attackMaxRange;

        chargeRange = profile.aggroRadius;
        attackRange = profile.leashRadius;

        // Damage assignments
        meleeDamage = profile.damage;
        rangedDamage = profile.damage;

        //Windup times
        meleeWindupTime = 10 / 36f;
        meleeRecoveryTime = 26 / 36f;
        rangedWindupTime = 25f / 45f;
        rangedRecoveryTime = 20f / 45f;

        // Cooldowns
        meleeCooldown = profile.baseAttackCooldown * meleeCooldownMultiplier;
        rangedCooldown = profile.baseAttackCooldown * rangedCooldownMultiplier;

        // Projectiles
        projectilePrefab = profile.projectilePrefab;

        projectileSpeed = profile.projectileSpeed;
        projectileLifetime = profile.projectileLifetime;

        // Particles off by default
        if (weaponParticles != null)
            weaponParticles.SetActive(false);

        // Super cooldown
        StartCoroutine(SuperCooldown(superAttackCooldown));

        initialized = true;
    }

    public void Tick(EnemyContext context) {
        if (!initialized || context.state == EnemyState.Dead || !playerInBossRoom) return;

        agent = context.agent;
        profile = context.profile ?? profile;
        player = context.player ?? player;

        if (player == null || agent == null || profile == null)
            return;

        float distToPlayer = context.distToPlayer;

        if (distToPlayer > profile.leashRadius)
            return;

        if (!attackReady)
            return;

        // Determine bands
        bool inMeleeBand = (distToPlayer >= meleeMin && distToPlayer <= meleeMax);
        bool inRangedBand = (distToPlayer > chargeRange && distToPlayer <= attackRange);

        // Choose correct attack
        if (inMeleeBand) {
            bool isCrit = Mathf.Abs(distToPlayer - meleeIdeal) <= 0.25f;

            if (isCrit)
                StartCoroutine(SetupAttack(AttackType.CritMelee));
            else
                StartCoroutine(SetupAttack(AttackType.Melee));
        }
        else if (inRangedBand) {
            // Roll super attack first
            if (superReady && Random.value < superAttackChance) {
                superReady = false;
                StartCoroutine(SetupAttack(AttackType.Super));
            }
            else {
                StartCoroutine(SetupAttack(AttackType.Ranged));
            }
        }
    }

    #endregion

    #region Attack Handlers

    void DoMeleeAttack() {
        if (player == null) return;

        PlayerAbilities abilities = player.GetComponent<PlayerAbilities>();
        if (abilities != null)
            abilities.RemoveFromHealth(meleeDamage);

        agent.updateRotation = true;
        StartCoroutine(Cooldown(meleeCooldown));
    }

    void DoCritMeleeAttack() {
        if (player == null) return;

        int critDamage = Mathf.RoundToInt(meleeDamage * critMultiplier);

        PlayerAbilities abilities = player.GetComponent<PlayerAbilities>();
        if (abilities != null)
            abilities.RemoveFromHealth(critDamage);

        agent.updateRotation = true;
        StartCoroutine(Cooldown(meleeCooldown));
    }

    void DoRangedAttack(Vector3 targetPos) {
        if (projectilePrefab == null || projectileSpawnPoint == null) {
            Debug.LogWarning($"{name}: Missing projectile prefab or spawn point.");
            StartCoroutine(Cooldown(rangedCooldown));
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        var mover = proj.GetComponent<EnemyProjectileMover>();
        if (mover != null)
            mover.SetAttributes(projectileSpeed, projectileLifetime, targetPos, rangedDamage);

        agent.updateRotation = true;
        StartCoroutine(Cooldown(rangedCooldown));
    }

    IEnumerator DoSuperAttack() {
        if (weaponParticles != null)
            weaponParticles.SetActive(true);

        agent.updateRotation = false;

        Vector3 targetPos = PlayerTargetPosition();

        FaceTarget(targetPos);

        if (animator != null)
            animator.SetTrigger("superAttack");

        // Volley 1
        SpawnProjectile(targetPos);

        // Volley 2
        targetPos = PlayerTargetPosition();
        yield return new WaitForSeconds(rangedRecoveryTime + rangedWindupTime + 0.13f);
        SpawnProjectile(targetPos + new Vector3(0, 0.5f, 0));
        SpawnProjectile(targetPos + new Vector3(0, -0.5f, 0));

        // Volley 3
        targetPos = PlayerTargetPosition();
        yield return new WaitForSeconds(rangedRecoveryTime + meleeWindupTime + 0.2f);
        SpawnProjectile(targetPos + new Vector3(0, 1f, 0));
        SpawnProjectile(targetPos);
        SpawnProjectile(targetPos + new Vector3(0, -1f, 0));

        if (weaponParticles != null)
            weaponParticles.SetActive(false);

        agent.updateRotation = true;

        StartCoroutine(Cooldown(rangedCooldown));

        // Super cooldown
        StartCoroutine(SuperCooldown(superAttackCooldown));
    }

    void SpawnProjectile(Vector3 pos) {
        if (projectilePrefab == null || projectileSpawnPoint == null)
            return;

        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        var mover = proj.GetComponent<EnemyProjectileMover>();
        if (mover != null)
            mover.SetAttributes(projectileSpeed, projectileLifetime, pos, rangedDamage);
    }

    #endregion

    #region Utility Methods

    IEnumerator SetupAttack(AttackType attackType) {
        attackReady = false;

        if (animator != null)
            animator.CrossFade("Idle", 0f);

        Vector3 targetPos = PlayerTargetPosition();

        // Snap rotation to player
        agent.updateRotation = false;
        FaceTarget(targetPos);

        // Animation signal
        if (animator != null)
            if (attackType == AttackType.Melee || attackType == AttackType.CritMelee)
                animator.SetTrigger("meleeAttack");
            else if (attackType == AttackType.Ranged)
                animator.SetTrigger("rangedAttack");
            else if (attackType == AttackType.Super)
                animator.SetTrigger("superAttack");

        float windupTime = attackType == AttackType.Ranged || attackType == AttackType.Super ? rangedWindupTime : meleeWindupTime;

        yield return new WaitForSeconds(windupTime);

        switch (attackType) {
            case AttackType.Melee:
                DoMeleeAttack();
                break;

            case AttackType.CritMelee:
                DoCritMeleeAttack();
                break;

            case AttackType.Ranged:
                DoRangedAttack(targetPos);
                break;

            case AttackType.Super:
                yield return StartCoroutine(DoSuperAttack());
                break;
        }
    }

    Vector3 PlayerTargetPosition() {
        return player != null
            ? player.position + new Vector3(0f, 2f, 0f)
            : transform.position + transform.forward * 5f;
    }

    void FaceTarget(Vector3 target) {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) {
            Quaternion look = Quaternion.LookRotation(dir);
            transform.rotation = look;
        }
    }

    IEnumerator Cooldown(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        attackReady = true;
    }

    IEnumerator SuperCooldown(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        superReady = true;
    }

    #endregion
}
