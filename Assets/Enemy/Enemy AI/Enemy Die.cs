using UnityEngine;



[RequireComponent(typeof(Animator))]
public class EnemyDie : MonoBehaviour {
    // DELETE THE BOSS SCRIPT, tack this one onto boss prefab, and set up a listener for boss death to do load screen stuff

    [Header("Settings")]
    [SerializeField] bool isBoss = false;

    Animator animator;
    BossRoomBase bossRoom;

    public BossRoomBase BossRoom { get => bossRoom; set => bossRoom = value; }

    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Die() {
        //play death animation
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            animator.CrossFade("Idle", 0.01f);
        }
        animator.SetTrigger("die");

        //Turn off collider
        GetComponent<Collider>().enabled = false;

        //Turn off AI
        IBehave[] behaviors = GetComponents<IBehave>();
        foreach (IBehave behavior in behaviors) {
            MonoBehaviour mb = behavior as MonoBehaviour;
            if (mb != null) {
                mb.enabled = false;
            }
        }

        //Turn off Hud and health
        GetComponent<EnemyHealthUI>().enabled = false;
        GetComponent<EnemyHealth>().enabled = false;
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        if (isBoss && bossRoom != null) {
            //Let boss fight controller know if this is a boss
            bossRoom.NotifyBossDefeated();
        }
        else {
            Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.5f);
        }
    }
}