using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    [SerializeField] public GameObject obstacle; //put in inspector
    [SerializeField] public WeaponType itemNeeded;

    public void OnTriggerEnter(Collider other) //in case dont want to click
    {
        if (other.gameObject.CompareTag("Player"))  //only if player enters it
        {
            ItemSO currentItemSO = EquipmentManager.instance.currentItem; //item holding, if holding
            if (currentItemSO.weaponType == itemNeeded) //if the held item is the same as the one needed to destroy
            {
                DestroyItem(); 
            }
        }
    }
    public void DestroyItem() //can be called from attack
    {
        Destroy(obstacle);
    }
}