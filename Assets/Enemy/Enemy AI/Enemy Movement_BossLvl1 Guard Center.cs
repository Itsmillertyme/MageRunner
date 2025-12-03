using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement_BossLvl1GuardCenter : MonoBehaviour, IEnemyMovementBehaviour {
    #region Variables
    [Header("Guard Settings")]
    [SerializeField] Vector3 guardPosition;
    [SerializeField] bool aiDebugMode;
    [SerializeField] bool spawningDebugMode;

    NavMeshAgent agent;
    Animator animator;
    EnemyProfile profile;

    bool initialized;

    #endregion

    #region Interface Methods

    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {
        this.spawningDebugMode = spawningDebugMode;
        this.aiDebugMode = aiDebugMode;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        Vector3 desiredGuard = transform.position;

        if (roomDataIn != null && roomDataIn.PathNode != null) {
            // Use room center
            desiredGuard = new Vector3(
                roomDataIn.PathNode.position.x,
                roomDataIn.EnemySpawns[0].position.y,
                transform.position.z
            );
        }

        // Snap to nearest NavMesh position
        if (NavMesh.SamplePosition(desiredGuard, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            guardPosition = hit.position;
        else
            guardPosition = desiredGuard;

        initialized = true;
    }

    public void Tick(EnemyContext context) {
        if (!initialized || context.agent == null || context.state == EnemyState.Dead)
            return;

        // Cache context references each tick
        profile = context.profile;
        agent = context.agent;

        // Update animator based on velocity
        if (animator != null)
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f && !agent.isStopped);

        // Movement state machine
        switch (context.state) {
            case EnemyState.Idle:
                HandleIdle(context);
                break;
            case EnemyState.Chase:
                HandleChase(context);
                break;
            case EnemyState.Combat:
                HandleCombatHold(context);
                break;
            case EnemyState.BackOff:
                HandleBackOff(context);
                break;
            case EnemyState.Return:
                HandleReturn(context);
                break;
        }

        if (aiDebugMode)
            Debug.Log($"{name} (BossGuardCenter) is currently in the state: {context.state}");
    }

    #endregion

    #region Movement Handlers

    void HandleIdle(EnemyContext context) {
        if (Vector3.Distance(transform.position, guardPosition) > agent.stoppingDistance)
            agent.SetDestination(guardPosition);
        else
            agent.SetDestination(transform.position);
    }

    void HandleChase(EnemyContext context) {
        if (context.player == null) return;

        // Move toward player until within ideal attack range
        if (context.distToPlayer > profile.attackIdealRange)
            agent.SetDestination(context.player.position);
        else
            agent.SetDestination(transform.position);
    }

    void HandleCombatHold(EnemyContext context) {
        if (context.player == null) return;

        float distance = context.distToPlayer;
        float ideal = profile.attackIdealRange;

        // Fine tune boss positioning around ideal attack distance
        if (distance > ideal + 0.25f) {
            agent.SetDestination(context.player.position);
        }
        else if (distance < ideal - 0.25f) {
            Vector3 dir = (transform.position - context.player.position).normalized;
            Vector3 back = transform.position + dir * 1.0f;
            agent.SetDestination(back);
        }
        else {
            agent.SetDestination(transform.position);
        }
    }

    void HandleBackOff(EnemyContext context) {
        if (context.player == null) return;

        // Retreat
        Vector3 away = (transform.position - context.player.position).normalized;
        Vector3 retreatTarget = transform.position + away * 5f;
        agent.SetDestination(retreatTarget);
    }

    void HandleReturn(EnemyContext context) {
        // Return to center guard position when leash broken
        if (Vector3.Distance(transform.position, guardPosition) > agent.stoppingDistance)
            agent.SetDestination(guardPosition);
        else
            agent.SetDestination(transform.position);
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        // Try to pick profile if not cached (so gizmos work in editor)
        if (profile == null) {
            EnemyBrain brain = GetComponent<EnemyBrain>();
            if (brain != null)
                profile = brain.Profile;
        }

        if (profile == null || !profile.showMovementGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(
            new Vector3(guardPosition.x, guardPosition.y + 0.5f, guardPosition.z),
            1f
        );
    }

    #endregion
}
