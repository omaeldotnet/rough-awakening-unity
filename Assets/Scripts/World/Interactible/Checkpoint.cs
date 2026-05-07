using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("PlayerX", collision.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", collision.gameObject.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", collision.gameObject.transform.position.z);
            Debug.Log("checkpoint set");
        }
    }

}
