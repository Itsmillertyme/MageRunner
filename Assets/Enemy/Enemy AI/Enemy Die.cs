using UnityEngine;
using UnityEngine.Events;

public class EnemyDie : MonoBehaviour
{
    // DELETE THE BOSS SCRIPT, tack this one onto boss prefab, and set up a listener for boss death to do load screen stuff

    [Header("Component References")]
    [SerializeField] Animator animator;

    private UnityEvent levelComplete;

    public void Die()
    {
        //play death animation
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.CrossFade("Idle", 0f);
        }
        animator.SetTrigger("die");
        GetComponent<Collider>().enabled = false;
    }
}