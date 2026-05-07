using UnityEngine;

public class CollideKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Health>().Damage(100);
        }
    }
}
