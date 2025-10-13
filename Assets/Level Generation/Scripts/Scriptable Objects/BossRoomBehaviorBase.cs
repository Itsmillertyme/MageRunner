using System.Collections;
using UnityEngine;

public abstract class BossRoomBehaviorBase : ScriptableObject {
    #region Data: Audio & Flow
    [Header("Audio")]
    [Tooltip("Pass to the room to play via music controller.")]
    public AudioClip bossMusic;

    [Header("Flow")]
    [Tooltip("Delay before the actual fight starts (camera pan, VFX, etc.).")]
    public float introDelay = 1.0f;

    [Tooltip("Auto close the door when the player enters.")]
    public bool autoCloseDoorOnEnter = true;

    [Tooltip("Auto open the door when the boss is defeated.")]
    public bool autoOpenDoorOnDefeat = true;

    [Tooltip("Show platforms when the player enters")]
    public bool showPlatformsOnEnter = true;

    [Tooltip("Hide platforms right after initialization")]
    public bool hidePlatformsOnStart = false;

    [Tooltip("If true, ActivateHazards() will be invoked during the entry flow.")]
    public bool useHazards = false;
    #endregion

    #region Lifecycle Hooks (Director)
    // Called from room.Initialize(). Good place to set initial room state.
    public virtual void OnInitialized(BossRoomBase room) {
        if (hidePlatformsOnStart) room.HidePlatforms();
    }
    // Called by the room when the player enters the trigger. This method *directs* the sequence.
    public virtual void OnEnter(BossRoomBase room) {
        if (autoCloseDoorOnEnter) room.CloseDoor();
        if (showPlatformsOnEnter) room.ShowPlatforms();

        room.BeginPreFightSequence();
        room.StartCoroutine(EnterRoutine(room));
    }
    // Called by the room when the boss reports defeat. This method *directs* the end sequence.
    public virtual void OnBossDefeated(BossRoomBase room) {
        // End fight & cleanup first
        room.EndBossFight();

        // Open the door if configured
        if (autoOpenDoorOnDefeat) room.OpenDoor();

        //hide platforms
        room.HidePlatforms();

        // Notify listeners
        room.RaiseBossDefeatedEvent();
    }
    #endregion

    #region Director Coroutine
    protected virtual IEnumerator EnterRoutine(BossRoomBase room) {
        if (introDelay > 0f)
            yield return new WaitForSeconds(introDelay);

        if (useHazards)
            room.ActivateHazards();

        if (bossMusic)
            room.PlayBossMusic(bossMusic);

        room.StartBossFight();
    }
    #endregion

    #region Helpers
    protected void TryPlaySFX(AudioClip clip) {
        if (!clip) return;
        var go = new GameObject("SFX_Temp");
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.clip = clip;
        src.spatialBlend = 0f;
        src.Play();
        Object.Destroy(go, clip.length + 0.1f);
    }
    #endregion
}
