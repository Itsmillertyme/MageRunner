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
        BossInstance.GetComponent<EnemyBrain>().InCutscene = true;

        //Turn off Player input and trigger anim
        playerController.InCutscene = true;
        playerController.gameObject.GetComponentInChildren<Animator>().SetBool("IsWalking", false);
        playerController.gameObject.GetComponentInChildren<Animator>().CrossFade("Idle", 0f);

        //BASTARDRY, Only for demo, turn off hud
        playerController.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        playerController.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
        playerController.transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(false);
        playerController.transform.GetChild(1).transform.GetChild(4).gameObject.SetActive(false);

        //trigger boss taunt anim
        BossInstance.GetComponent<Animator>().SetTrigger("taunt");

        //trigger dialogue

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
        BossInstance.GetComponent<EnemyBrain>().InCutscene = false;
        BossInstance.GetComponent<EnemyCombat_BossLvl1>().PlayerInBossRoom = true;

        //enable player 
        playerController.InCutscene = false;


        //BASTARDRY, Only for demo, turn on hud
        playerController.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
        playerController.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
        playerController.transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(true);
        playerController.transform.GetChild(1).transform.GetChild(4).gameObject.SetActive(true);
    }

    public override void ActivateHazards() {
        // dissapearing platforms
        Debug.Log($"[Boss Fight] Hazards activated");
    }

    public override void EndBossFight() {
        Debug.Log($"[Boss Fight] Boss fight cleanup");

        // Cleanup hazards
        base.EndBossFight();

        float delay = BossInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length + 1.5f;

        Destroy(BossInstance, delay);

        //StartCoroutine(GoToMainMenu());
        //show demo outro menu
        StartCoroutine(ShowDemoOutroPanel(delay + 3));
    }

    public override void PlayBossMusic(AudioClip clip) {
        // Implement after music controller updated
    }
    #endregion

    public void TriggerDialogue(DialogueData dialogue) {
        playerController.GetComponent<PlayerDialogueDriver>().TriggerDialogue(dialogue);
    }

    IEnumerator GoToMainMenu() {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Splash");
    }

    IEnumerator ShowDemoOutroPanel(float delay) {
        yield return new WaitForSeconds(delay);

        DemoOutroPanelController outro = FindAnyObjectByType<DemoOutroPanelController>(FindObjectsInactive.Include);
        outro.ShowPanel(0.33f);
    }
    IEnumerator ResetCinemachineBlend(float newBlendTime) {
        CinemachineBrain cb = FindAnyObjectByType<CinemachineBrain>();
        yield return new WaitForSeconds(cb.m_DefaultBlend.m_Time);
        cb.m_DefaultBlend.m_Time = newBlendTime;
    }
}
