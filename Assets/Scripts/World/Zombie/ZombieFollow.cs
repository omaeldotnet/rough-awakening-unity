using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    public float moveSpeed; // Adjust speed here
    private Animator animator;

    public GameObject target;
    private NavMeshAgent agent;
    public float normalSpeed = 3f;
    public float runningSpeed = 4.5f;

    public bool isFollowing = false;
    public bool playerGoingAway = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = true;
        animator.SetBool("canWalk", false);
    }

void Update()
{
    if (isFollowing && agent != null && agent.isOnNavMesh)
    {
        agent.isStopped = false;
        agent.destination = target.transform.position;
        agent.speed = playerGoingAway ? runningSpeed : normalSpeed;
        animator.SetBool("canWalk", true);
        Debug.LogWarning("going to : " + agent.destination);
    }
}


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerGoingAway = true;
        }
    }


    public void FollowPlayer()
    {
        isFollowing = true;
    }

    
}