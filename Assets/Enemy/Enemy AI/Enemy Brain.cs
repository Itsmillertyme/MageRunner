using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBrain : MonoBehaviour, IBehave {

    #region Variables
    [Header("Config")]
    [SerializeField] EnemyProfile profile;
    [SerializeField] Transform player;

    [Header("Behaviour Components")]
    [SerializeField] MonoBehaviour movementBehaviourSource;
    [SerializeField] MonoBehaviour combatBehaviourSource;

    [Header("Debug")]
    [SerializeField] bool spawningDebugMode;
    [SerializeField] bool aiDebugMode;

    NavMeshAgent agent;
    IEnemyMovementBehaviour movementBehaviour;
    IEnemyCombatBehaviour combatBehaviour;

    EnemyState currentState = EnemyState.Idle;
    RoomData roomData;
    bool initialized;

    public EnemyProfile Profile => profile;

    #endregion

    #region Unity Methods
    void Awake() {
        //Get Navmesh reference
        agent = GetComponent<NavMeshAgent>();

        // setup navmesh agent from profile SO
        if (profile != null) {
            agent.speed = profile.moveSpeed;
            agent.angularSpeed = profile.angularSpeed;
            agent.updateRotation = !profile.use2DFlipFacing;
        }

        movementBehaviour = movementBehaviourSource as IEnemyMovementBehaviour;
        combatBehaviour = combatBehaviourSource as IEnemyCombatBehaviour;

        //DEBUG
        if (movementBehaviour == null && movementBehaviourSource != null) {
            if (aiDebugMode) Debug.LogError($"{name}: Movement behaviour source does not implement IEnemyMovementBehaviour.");
        }
        if (combatBehaviour == null && combatBehaviourSource != null) {
            if (aiDebugMode) Debug.LogError($"{name}: Combat behaviour source does not implement IEnemyCombatBehaviour.");
        }

        //Get player reference
        if (player == null) {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) player = playerGO.transform;
        }
    }

    void Update() {
        if (!initialized || player == null || currentState == EnemyState.Dead)
            return;

        //Get player distance
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // Get current state 
        UpdateState(distToPlayer);

        // Setup reference context
        EnemyContext ctx = new EnemyContext {
            enemy = transform,
            player = player,
            distToPlayer = distToPlayer,
            profile = profile,
            agent = agent,
            state = currentState,
            aiDebug = aiDebugMode
        };

        // Tick behaviours
        movementBehaviour?.Tick(ctx);
        combatBehaviour?.Tick(ctx);

        // Do flip?
        if (profile != null && profile.use2DFlipFacing && player != null) {
            UpdateFacing2D();
        }
    }
    #endregion

    #region Utility Methods
    public void Initialize(RoomData roomDataIn, bool spawningDebugMode = false, bool aiDebugMode = false) {

        if (initialized) return;

        this.roomData = roomDataIn;
        this.spawningDebugMode = spawningDebugMode;
        this.aiDebugMode = aiDebugMode;

        // pass init down to behaviours
        movementBehaviour?.Initialize(roomDataIn, spawningDebugMode, aiDebugMode);
        combatBehaviour?.Initialize(roomDataIn, spawningDebugMode, aiDebugMode);

        //rotate visual container if using 2D flip
        Transform visualTransform = transform.GetChild(0).transform;
        if (profile.use2DFlipFacing) {
            visualTransform.rotation = Quaternion.Euler(visualTransform.rotation.x, 90, visualTransform.rotation.z);
        }
        else {
            visualTransform.rotation = Quaternion.Euler(visualTransform.rotation.x, 0, visualTransform.rotation.z);
        }

        initialized = true;
    }

    void UpdateState(float distToPlayer) {
        // Radius checks

        // Outside leash radius
        if (distToPlayer > profile.leashRadius) {
            currentState = EnemyState.Return;
            return;
        }

        // Not yet in aggro radius
        if (distToPlayer > profile.aggroRadius) {
            if (currentState != EnemyState.Return)
                currentState = EnemyState.Idle;
            return;
        }

        // Inside aggro radius 
        if (distToPlayer < profile.attackMinRange) {
            currentState = EnemyState.BackOff;
        }
        else if (distToPlayer <= profile.attackMaxRange) {
            currentState = EnemyState.Combat;
        }
        else {
            currentState = EnemyState.Chase;
        }
    }

    void UpdateFacing2D() {
        float distanceX = player.position.x - transform.position.x;
        if (Mathf.Approximately(distanceX, 0f)) return;

        Vector3 scale = transform.localScale;
        bool facingRight = scale.x > 0f;

        // Flip based on player position
        if (distanceX > 0f && !facingRight) {
            //face right
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        else if (distanceX < 0f && facingRight) {
            // Face left
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    #endregion
}

#region Enums & Structs
public enum EnemyState {
    Idle,
    Chase,
    Combat,
    BackOff,
    Flee,
    Return,
    Dead
}

public struct EnemyContext {
    public Transform enemy;
    public Transform player;
    public float distToPlayer;
    public EnemyProfile profile;
    public NavMeshAgent agent;
    public EnemyState state;
    public bool aiDebug;
}
#endregion
