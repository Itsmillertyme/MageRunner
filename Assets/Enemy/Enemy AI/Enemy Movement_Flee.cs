using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement_Flee : MonoBehaviour, IEnemyMovementBehaviour {
    #region Variables
    [Header("Flee Settings")]
    [SerializeField] float navSampleRadius = 2f;
    [SerializeField] float returnCooldown = 2f;

    private Vector3 startingPosition;
    private Vector3 fleeTarget;

    private bool initialized = false;
    private bool hasFleeTarget = false;
    private float returnTimer = 0f;

    private RoomData room;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyProfile profile;

    private bool spawningDebugMode;
    private bool aiDebugMode;
    #endregion

    #region Interface Methods

    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {
        this.room = roomDataIn;
        this.spawningDebugMode = spawningDebugMode;
        this.aiDebugMode = aiDebugMode;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        startingPosition = transform.position;
        hasFleeTarget = false;
        returnTimer = 0f;

        initialized = true;
    }

    public void Tick(EnemyContext context) {
        if (!initialized) return;

        profile = context.profile;
        agent = context.agent;

        // Update walking animation
        if (animator != null)
            animator.SetBool("isWalking", !agent.isStopped && agent.velocity.magnitude > 0.1f);

        switch (context.state) {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Combat:
                HandleCombat();
                break;

            case EnemyState.BackOff:
                HandleFlee(context);
                break;

            case EnemyState.Return:
                HandleReturn();
                break;
        }

        if (aiDebugMode) Debug.Log($"{name} is currently in the state: {context.state}");
    }

    #endregion

    #region Movement Handlers

    private void HandleIdle() {
        agent.SetDestination(startingPosition);
        hasFleeTarget = false;
        returnTimer = 0f;
    }

    private void HandleCombat() {
        agent.SetDestination(transform.position);
    }

    private void HandleFlee(EnemyContext context) {
        if (context.player == null || room == null) return;

        //if (context.distToPlayer >= profile.attackMinRange) {
        //    agent.SetDestination(transform.position);
        //    return;
        //}

        if (!hasFleeTarget) {
            fleeTarget = GenerateQuadrantFleePoint(context);
            hasFleeTarget = true;
        }

        agent.SetDestination(fleeTarget);
    }

    private void HandleReturn() {

        returnTimer += Time.deltaTime;

        // Cooldown before returning home
        if (returnTimer < returnCooldown) {
            agent.SetDestination(transform.position);
            return;
        }

        // Back to guard position
        agent.SetDestination(startingPosition);

        if (Vector3.Distance(transform.position, startingPosition) <= 0.3f) {
            hasFleeTarget = false;
            returnTimer = 0f;
        }
    }
    #endregion

    #region Utiliy Methods
    private Vector3 GenerateQuadrantFleePoint(EnemyContext context) {
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = context.player.position;

        float minX = room.TopLeftObject.position.x;
        float maxX = room.BottomRightObject.position.x;
        float minY = room.BottomRightObject.position.y;
        float maxY = room.TopLeftObject.position.y;

        float targetMinX, targetMaxX;
        float targetMinY, targetMaxY;

        // Horizontal opposite
        if (playerPosition.x >= enemyPosition.x) {
            targetMinX = minX;
            targetMaxX = enemyPosition.x;
        }
        else {
            targetMinX = enemyPosition.x;
            targetMaxX = maxX;
        }

        // Vertical opposite
        if (playerPosition.y >= enemyPosition.y) {
            targetMinY = minY;
            targetMaxY = enemyPosition.y;
        }
        else {
            targetMinY = enemyPosition.y;
            targetMaxY = maxY;
        }

        // Make sure range tiny
        if (Mathf.Approximately(targetMinX, targetMaxX))
            targetMaxX += 1f;
        if (Mathf.Approximately(targetMinY, targetMaxY))
            targetMaxY += 1f;

        // Pick random point in that quadrant
        Vector3 candidate = new Vector3(
            Random.Range(targetMinX, targetMaxX),
            Random.Range(targetMinY, targetMaxY),
            enemyPosition.z
        );

        // NavMesh validation
        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, navSampleRadius, NavMesh.AllAreas))
            return hit.position;

        // Fallback 
        Vector3 quadrantCenter = new Vector3((targetMinX + targetMaxX) * 0.5f, (targetMinY + targetMaxY) * 0.5f, enemyPosition.z
        );

        if (NavMesh.SamplePosition(quadrantCenter, out hit, navSampleRadius, NavMesh.AllAreas))
            return hit.position;

        return enemyPosition;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected() {
        if (profile == null) {
            EnemyBrain brain = GetComponent<EnemyBrain>();
            if (brain != null)
                profile = brain.Profile;
        }
        if (!profile.showMovementGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(fleeTarget + Vector3.up, 0.3f);
    }
    #endregion
}
