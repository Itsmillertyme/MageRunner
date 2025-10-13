using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _01Castle_BossRoom : BossRoomBase {
    #region Fight Flow 
    public override void BeginPreFightSequence() {
        base.BeginPreFightSequence();
        // Castle-specific pre-fight
        BossInstance.GetComponent<Lvl1BossCombat>().InCutscene = true;
        BossInstance.GetComponent<Lvl1BossGuardCenter>().InCutscene = true;
        BossInstance.GetComponent<Animator>().SetTrigger("taunt");

        playerController.InCutscene = true;

        //TODO:
        //turn off music manager mcurrent track

        Debug.Log($"[Boss Fight] Pre-Fight sequence begun");

    }

    public override void StartBossFight() {
        base.StartBossFight();

        // Enable boss AI
        BossInstance.GetComponent<Lvl1BossCombat>().InCutscene = true;
        BossInstance.GetComponent<Lvl1BossGuardCenter>().InCutscene = true;

        //enable player
        playerController.InCutscene = false;

        Debug.Log($"[Boss Fight] Boss fight started");
    }

    public override void ActivateHazards() {
        // dissapearing platforms
        Debug.Log($"[Boss Fight] Hazards activated");
    }

    public override void EndBossFight() {
        Debug.Log($"[Boss Fight] Boss fight cleanup");

        // Cleanup hazards
        base.EndBossFight();

        Destroy(BossInstance, 1.5f);

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

}
