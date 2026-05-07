using UnityEngine;

public class Consumable : MonoBehaviour
{
    public ConsumableType type;
    public int healAmount = 20;
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f); // degrees per second

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                if (type == ConsumableType.Food)
                {
                    health.Heal(healAmount);
                }
                else if (type == ConsumableType.Medkit)
                {
                    health.HealFull();
                }

                Destroy(gameObject); // Remove the consumable from the scene
            }
        }
    }
}
