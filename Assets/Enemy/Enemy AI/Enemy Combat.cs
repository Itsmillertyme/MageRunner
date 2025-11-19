using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class EnemyCombat : MonoBehaviour, IEnemyCombatBehaviour {
    #region Variables
    [Header("Component References")]
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform projectileSpawnPoint;

    bool initialized;
    bool attackReady = true;
    bool isAttacking;
    bool aiDebugMode;
    bool spawningDebugMode;

    EnemyProfile profile;
    Transform player;
    #endregion

    #region Interface Methods
    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {
        this.spawningDebugMode = spawningDebugMode;
        this.aiDebugMode = aiDebugMode;

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) player = playerGO.transform;
        }

        initialized = true;
    }

    public void Tick(EnemyContext context) {
        if (!initialized || isAttacking) return;

        // Get context this tick
        profile = context.profile;
        player = context.player;
        agent = context.agent;

        if (profile == null || player == null) return;

        float cooldown = Mathf.Max(0.1f, profile.baseAttackCooldown);

        // Only attack while in Combat state
        if (context.state == EnemyState.Combat && attackReady) {

            // Recheck distances
            if (context.distToPlayer <= profile.attackMaxRange &&
                context.distToPlayer >= profile.attackMinRange) {
                StartCoroutine(PerformAttack(cooldown));
            }
        }
    }
    #endregion

    #region Attacks
    IEnumerator PerformAttack(float cooldown) {
        attackReady = false;
        isAttacking = true;

        // Snap idle animation if needed
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            animator.CrossFade("Idle", 0f);

        // Trigger attack animation
        if (animator != null)
            animator.SetTrigger("attack");

        // Look at player 
        if (!profile.use2DFlipFacing && player != null) {
            Vector3 dir = (player.position - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        // Wait for wind-up (hit frame)
        yield return new WaitForSeconds(profile.windUpTime);

        if (profile.attackType == AttackType.Melee)
            DoMeleeAttack();
        else if (profile.attackType == AttackType.Ranged)
            DoRangedAttack();

        // Recovery window before another action
        yield return new WaitForSeconds(profile.recoveryTime + cooldown);

        isAttacking = false;
        attackReady = true;
    }

    void DoMeleeAttack() {
        if (player == null) return;

        Vector3 center = transform.position + transform.forward * (profile.attackRadius * 0.75f) + Vector3.up;
        Collider[] hits = Physics.OverlapSphere(center, profile.attackRadius, LayerMask.GetMask("Player"));

        foreach (var hit in hits) {
            PlayerAbilities ph = hit.GetComponent<PlayerAbilities>();
            if (ph != null) {
                ph.RemoveFromHealth(profile.damage);
                if (aiDebugMode)
                    Debug.Log($"[EnemyCombat] {name} hit {hit.name} for {profile.damage} damage.");
            }
        }

        if (profile.showCombatGizmos)
            Debug.DrawRay(center, transform.forward * profile.attackRadius, Color.red, 0.25f);
    }

    void DoRangedAttack() {
        if (profile.projectilePrefab == null || projectileSpawnPoint == null) return;

        Vector3 targetPos = player.position + Vector3.up;
        GameObject projectile = Instantiate(profile.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        EnemyProjectileMover mover = projectile.GetComponent<EnemyProjectileMover>();
        if (mover != null)
            mover.SetAttributes(profile.projectileSpeed, profile.projectileLifetime, targetPos, profile.damage);

        if (aiDebugMode)
            Debug.Log($"[EnemyCombat] {name} fired projectile toward {targetPos}");
    }
    #endregion
}
