using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackCooldownSeconds;
    private float lastAttackTime;
    private ZombieAudio zombieAudio;
    private Animator animator;
    private bool playerInRange;

    private void Start()
    {
        animator = GetComponent<Animator>();
        zombieAudio = GetComponentInParent<ZombieAudio>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            animator.SetBool("isAttacking", true);
        
        }
    }

private void OnTriggerStay(Collider collision)
{
    if (playerInRange && Time.time > lastAttackTime + attackCooldownSeconds)
    {
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.Damage(10);
            lastAttackTime = Time.time;

            if (zombieAudio != null)
            {
                zombieAudio.PlayAttackSound();
            }
        }
    }
}


    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            animator.SetBool("isAttacking", false);
            
        }
    }
}
