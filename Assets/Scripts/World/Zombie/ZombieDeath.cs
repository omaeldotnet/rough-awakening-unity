using UnityEngine;
using UnityEngine.AI;

public class ZombieDeath : MonoBehaviour
{
    private ZombieAudio zombieAudio;
    private Animator animator;
    private NavMeshAgent agent;
    private Health health;

    private void Start()
    {
        zombieAudio = GetComponent<ZombieAudio>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        if (health != null)
        {
            health.Died.AddListener(OnDeath); //add listener automatic pour quand que zombie die
        } //fonction onDeath va run quand zombie hp = 0, died invoke
    }

    private void OnDeath()
    {
        if (zombieAudio != null)
        {
            zombieAudio.PlayDeathSound();
        }

        if (agent != null)
        {
            agent.enabled = false;
        }

        animator.SetBool("isAttacking", false); //put everything off to be clean
        animator.SetBool("canWalk", false);
        animator.SetTrigger("die"); //starts death anim

        Destroy(gameObject, 3f); // destroy after a delay
    }
}
