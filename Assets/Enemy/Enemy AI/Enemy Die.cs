using UnityEngine;



[RequireComponent(typeof(Animator))]
public class EnemyDie : MonoBehaviour {
    // DELETE THE BOSS SCRIPT, tack this one onto boss prefab, and set up a listener for boss death to do load screen stuff

    [Header("Component References")]
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
            animator.CrossFade("Idle", 0f);
        }
        animator.SetTrigger("die");
        GetComponent<Collider>().enabled = false;

        //Let boss fight controller know if this is a boss
        if (isBoss && bossRoom != null) bossRoom.NotifyBossDefeated();
    }
}