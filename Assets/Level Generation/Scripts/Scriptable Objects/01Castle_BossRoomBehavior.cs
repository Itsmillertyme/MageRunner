using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "BossRooms/01Castle_BossRoomBehavior", fileName = "01Castle_BossRoomBehavior")]
public class _01Castle_BossRoomBehavior : BossRoomBehaviorBase {
    #region Castle Data
    [Header("Castle FX / SFX")]
    [Tooltip("Optional SFX to play during pre-fight buildup.")]
    public AudioClip introSFX;

    [Tooltip("Optional SFX to play after boss defeat.")]
    public AudioClip bossDefeatSFX;
    #endregion

    #region Director Overrides
    public override void OnEnter(BossRoomBase room) {
        // Standard door/platform setup
        if (autoCloseDoorOnEnter) room.CloseDoor();
        if (showPlatformsOnEnter) room.ShowPlatforms();

        // play intro sfx
        TryPlaySFX(introSFX);

        // Continue with unified sequence
        room.BeginPreFightSequence();
        room.StartCoroutine(EnterRoutine(room));
    }

    protected override IEnumerator EnterRoutine(BossRoomBase room) {

        //Level specifc timing here

        // Use base routine for timing/hazards/music/start
        yield return base.EnterRoutine(room);
    }

    public override void OnBossDefeated(BossRoomBase room) {
        Debug.Log($"[Boss Fight] Boss defeated");
        // Castle victory flair first
        TryPlaySFX(bossDefeatSFX);

        // Then run the standard end sequence (end fight, open door, raise event)
        base.OnBossDefeated(room);
    }
    #endregion
}
