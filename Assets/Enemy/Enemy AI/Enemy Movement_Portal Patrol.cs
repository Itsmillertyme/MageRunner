using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement_PortalPatrol : MonoBehaviour, IEnemyMovementBehaviour {
    #region Variables
    [Header("Portal Patrol Settings")]
    [SerializeField] float inwardOffset = 1.25f;
    [SerializeField] float waypointReachThreshold = 0.5f;
    [SerializeField] float navSampleRadius = 3f;
    [SerializeField] bool generateInteriorFallbackPoints = true;
    [SerializeField] float fallbackMinSpacing = 4f;
    [SerializeField] int fallbackPointCount = 2;

    List<Vector3> patrolPoints = new List<Vector3>();
    int currentWaypointIndex;
    bool initialized;
    bool spawningDebugMode;
    bool aiDebugMode;

    NavMeshAgent agent;
    Animator animator;
    EnemyProfile profile;
    #endregion

    #region Interface Methods
    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {
        this.spawningDebugMode = spawningDebugMode;
        this.aiDebugMode = aiDebugMode;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Generate patrol points using provided RoomData
        GeneratePortalPatrolPoints(roomDataIn);

        // Fallback if no portals resolved to valid NavMesh points
        if (patrolPoints.Count == 0 && generateInteriorFallbackPoints)
            GenerateFallbackInteriorPoints(roomDataIn);

        // If we successfully have at least one patrol point, begin patrol
        if (patrolPoints.Count > 0) {
            agent.SetDestination(patrolPoints[0]);
            initialized = true;
        }
        else {
            Debug.LogWarning($"[Enemy AI] No patrol points generated for {name}");
        }
    }
    public void Tick(EnemyContext context) {
        if (!initialized || context.agent == null) return;

        profile = context.profile;
        agent = context.agent;

        // Update animator walking state
        if (animator != null)
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f && !agent.isStopped);

        // Movement based on enemy state
        switch (context.state) {
            case EnemyState.Idle:
                HandlePatrolTraversal();
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

    // Patrol between points when idle
    void HandlePatrolTraversal() {
        if (patrolPoints.Count == 0) return;

        float dist = Vector3.Distance(transform.position, patrolPoints[currentWaypointIndex]);

        if (dist <= waypointReachThreshold) {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentWaypointIndex]);
        }
    }

    void HandleChase(EnemyContext context) {
        if (context.player == null) return;

        if (context.distToPlayer > profile.attackIdealRange)
            agent.SetDestination(context.player.position);
        else
            agent.SetDestination(transform.position);
    }

    void HandleCombatHold(EnemyContext context) {
        if (context.player == null) return;

        float distance = context.distToPlayer;
        float ideal = profile.attackIdealRange;

        if (distance > ideal + 0.25f)
            agent.SetDestination(context.player.position);
        else if (distance < ideal - 0.25f) {
            Vector3 dir = (transform.position - context.player.position).normalized;
            Vector3 back = transform.position + dir * 1f;
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
        // Simply resume patrol
        HandlePatrolTraversal();
    }

    #endregion

    #region Utiliy Methods

    void GeneratePortalPatrolPoints(RoomData room) {
        patrolPoints.Clear();

        if (room == null || room.Portals == null) return;

        foreach (var portal in room.Portals) {
            if (portal == null) continue;

            // Move slightly toward room center
            Vector3 directionToCenter = (new Vector3(room.PathNode.position.x, room.PathNode.position.y, -2.5f) - portal.transform.position).normalized; //Rooms always use -2.5 Z for center of walking plane
            Vector3 candidate = portal.transform.position + directionToCenter * inwardOffset;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, navSampleRadius, NavMesh.AllAreas)) {
                patrolPoints.Add(hit.position);
            }
            else {
                if (spawningDebugMode)
                    Debug.LogWarning($"[Enemy AI] Could not sample NavMesh near portal at {candidate} for {name}");
            }
        }
    }

    void GenerateFallbackInteriorPoints(RoomData room) {
        if (spawningDebugMode) Debug.Log($"[Enemy AI] Using Fall back points for {name}");

        Vector3 topLeft = room.TopLeftObject.position;
        Vector3 bottomRight = room.BottomRightObject.position;

        int attempts = 0;
        while (patrolPoints.Count < fallbackPointCount && attempts < 20) {
            attempts++;

            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(bottomRight.y, topLeft.y);
            Vector3 randomPoint = new Vector3(x, y, -2.5f); //-2.5Z is always center of plane

            //prevent clustering
            bool tooClose = false;
            foreach (Vector3 point in patrolPoints) {
                if (Vector3.Distance(point, randomPoint) < fallbackMinSpacing) {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose) continue;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, navSampleRadius, NavMesh.AllAreas)) {
                patrolPoints.Add(hit.position);
            }
        }
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
        for (int i = 0; i < patrolPoints.Count; i++) {

            if (i == currentWaypointIndex) {
                Gizmos.color = Color.red;
            }
            else {
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawSphere(patrolPoints[i], 1f);

            if (i > 0) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(patrolPoints[i - 1], patrolPoints[i]);
            }
        }
    }
    #endregion
}
