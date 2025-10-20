using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _01Castle_BossRoom : BossRoomBase {
    #region Fight Flow 
    public override void BeginPreFightSequence() {
        Debug.Log($"[Boss Fight] Pre-Fight sequence begun");

        base.BeginPreFightSequence();

        //set camera to focus on player and boss
        Vector3 playerLocation = playerController.gameObject.transform.position;
        Vector3 bossLocation = BossInstance.transform.position;
        GameObject midpoint = new GameObject("Temp Boss Room Object");
        midpoint.transform.position = (playerLocation + bossLocation) / 2f;
        //
        CinemachineBrain cb = FindAnyObjectByType<CinemachineBrain>();
        cb.m_DefaultBlend.m_Time = 0.25f;
        CameraController cc = FindAnyObjectByType<CameraController>();
        cc.SetToCutSceneCamera(midpoint.transform);

        //Turn off boss AI
        BossInstance.GetComponent<Lvl1BossCombat>().InCutscene = true;
        BossInstance.GetComponent<Lvl1BossGuardCenter>().InCutscene = true;
        //Turn off Player input
        playerController.InCutscene = true;

        //trigger taunt anim
        BossInstance.GetComponent<Animator>().SetTrigger("taunt");

        //TODO:
        //turn off music manager current track
    }

    public override void StartBossFight() {
        Debug.Log($"[Boss Fight] Boss fight started");

        base.StartBossFight();
        CameraController cc = FindAnyObjectByType<CameraController>();
        cc.SetToCurrentCamera(playerController.transform);
        StartCoroutine(ResetCinemachineBlend(2));

        // Enable boss AI
        BossInstance.GetComponent<Lvl1BossCombat>().InCutscene = false;
        BossInstance.GetComponent<Lvl1BossGuardCenter>().InCutscene = false;

        //enable player 
        playerController.InCutscene = false;
    }

    public override void ActivateHazards() {
        // dissapearing platforms
        Debug.Log($"[Boss Fight] Hazards activated");
    }

    public override void EndBossFight() {
        Debug.Log($"[Boss Fight] Boss fight cleanup");

        // Cleanup hazards
        base.EndBossFight();

        Destroy(BossInstance, BossInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length + 1.5f);

        StartCoroutine(GoToMainMenu());
    }

    public override void PlayBossMusic(AudioClip clip) {
        // Implement after music controller updated
    }
    #endregion

    IEnumerator GoToMainMenu() {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Splash");
    }

    IEnumerator ResetCinemachineBlend(float newBlendTime) {
        CinemachineBrain cb = FindAnyObjectByType<CinemachineBrain>();
        yield return new WaitForSeconds(cb.m_DefaultBlend.m_Time);
        cb.m_DefaultBlend.m_Time = newBlendTime;
    }
}
