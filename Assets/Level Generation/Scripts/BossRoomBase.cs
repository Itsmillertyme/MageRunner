using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BossRoomBase : MonoBehaviour {
    #region Inspector: Scene References
    [Header("Scene References")]
    [SerializeField] protected GameObject door;
    [SerializeField] protected List<GameObject> platforms = new List<GameObject>();
    #endregion

    #region Inspector: Behavior + Events
    [Header("Behavior (Director)")]
    [SerializeField] protected BossRoomBehaviorBase behavior;

    [Header("Events")]
    [SerializeField] protected GameEvent bossDefeatedEvent; // Raise via action
    #endregion

    protected PlayerController playerController; //set by Initialize

    #region Properties
    public GameObject BossInstance { get; private set; } //set by Initialize    
    #endregion

    #region Initialization & Unity Flow
    // One-time initialization from level generator
    public virtual void Initialize(GameObject bossInstance, PlayerController playerController, BossRoomBehaviorBase behaviorOverride = null) {
        BossInstance = bossInstance;
        this.playerController = playerController;

        if (behavior == null && behaviorOverride != null) { //if not set in inspector, use the override
            behavior = behaviorOverride;
        }
        behavior?.OnInitialized(this);
    }
    //
    protected virtual void Start() { }
    //
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        // SO controls the sequence.
        behavior?.OnEnter(this);
    }
    #endregion

    #region Actions
    public virtual void CloseDoor() {
        if (door) door.SetActive(true);
    }
    //
    public virtual void OpenDoor() {
        if (door) door.SetActive(false);
    }
    //
    public virtual void HidePlatforms() {
        foreach (GameObject platforms in platforms) if (platforms) platforms.SetActive(false);
    }
    //
    public virtual void ShowPlatforms() {
        foreach (GameObject platforms in platforms) if (platforms) platforms.SetActive(true);
    }
    #endregion

    #region Fight Flow 
    //Optional short cutscene / setup before the fight actually starts.</summary>
    public virtual void BeginPreFightSequence() {
        //override in room child.
    }

    //Start the actual boss fight
    public virtual void StartBossFight() {
        //override in room child.
    }

    //Turn on environmental hazards (if any)
    public virtual void ActivateHazards() {
        // Optional; none by default.
    }

    //Clean up hazards/effects at fight end
    public virtual void EndBossFight() {
        // Optional; none by default.
    }

    //Hook to pass music into your music system.
    public virtual void PlayBossMusic(AudioClip clip) {
        // Implement after music controller updated
    }

    //Raise the shared 'boss defeated' game event
    public virtual void RaiseBossDefeatedEvent() {
        bossDefeatedEvent?.Raise();
    }
    #endregion

    #region Boss → Room notification
    //Called by boss AI controller
    public void NotifyBossDefeated() {
        if (behavior != null) {
            behavior.OnBossDefeated(this);
        }
        else {
            // Default if no behavior set:
            EndBossFight();
            OpenDoor();
            RaiseBossDefeatedEvent();
        }
    }
    #endregion
}
