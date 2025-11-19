using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement_GuardChase : MonoBehaviour, IEnemyMovementBehaviour {
    #region Variables
    [Header("Guard Settings")]
    [SerializeField] Vector3 guardPosition;

    NavMeshAgent agent;
    Animator animator;
    bool initialized;
    bool aiDebugMode;
    bool spawningDebugMode;
    EnemyProfile profile;
    #endregion

    #region Interface Methods
    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {
        this.spawningDebugMode = spawningDebugMode;
        this.aiDebugMode = aiDebugMode;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Get guard position
        Vector3 desiredGuard = new Vector3(roomDataIn.PathNode.position.x, roomDataIn.PathNode.position.y, -2.5f);

        // Snap to nearest NavMesh position
        if (NavMesh.SamplePosition(desiredGuard, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            guardPosition = hit.position;
        else
            guardPosition = desiredGuard;

        initialized = true;
    }

    public void Tick(EnemyContext context) {
        if (!initialized || context.agent == null) return;

        //Get context references this tick
        profile = context.profile;
        agent = context.agent;

        // Update animator based on velocity
        if (animator != null)
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f && !agent.isStopped);

        //Movement state machine
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


        if (aiDebugMode) Debug.Log($"{name} is currently in the state: {context.state}");
    }
    #endregion

    #region Movement Handlers
    void HandleIdle(EnemyContext context) {
        // Stand at guard point or return if drifted
        if (Vector3.Distance(transform.position, guardPosition) > agent.stoppingDistance)
            agent.SetDestination(guardPosition);
        else
            agent.SetDestination(transform.position);
    }

    void HandleChase(EnemyContext context) {
        if (context.player == null) return;

        // Move toward player until within ideal range
        if (context.distToPlayer > profile.attackIdealRange)
            agent.SetDestination(context.player.position);
        else
            agent.SetDestination(transform.position);
    }

    void HandleCombatHold(EnemyContext context) {
        if (context.player == null) return;

        float distance = context.distToPlayer;
        float ideal = profile.attackIdealRange;

        // Fine tune distance adjustment for positioning
        if (distance > ideal + 0.25f)
            agent.SetDestination(context.player.position);
        else if (distance < ideal - 0.25f) {
            Vector3 dir = (transform.position - context.player.position).normalized;
            Vector3 back = transform.position + dir * 1.0f;
            agent.SetDestination(back);
        }
        else
            agent.SetDestination(transform.position);
    }

    void HandleBackOff(EnemyContext context) {
        if (context.player == null) return;

        Vector3 away = (transform.position - context.player.position).normalized;
        Vector3 retreatTarget = transform.position + away * 2f;
        agent.SetDestination(retreatTarget);
    }

    void HandleReturn(EnemyContext context) {
        if (Vector3.Distance(transform.position, guardPosition) > agent.stoppingDistance)
            agent.SetDestination(guardPosition);
        else
            agent.SetDestination(transform.position);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected() {
        if (!profile.showMovementGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(guardPosition.x, guardPosition.y + .5f, guardPosition.z), 1f);
    }
    #endregion
}