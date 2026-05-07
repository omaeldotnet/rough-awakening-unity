using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemSO item;
    private bool isPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (Inventory.instance.AddItem(item))
        {
            Destroy(gameObject);
        }
    }
}
