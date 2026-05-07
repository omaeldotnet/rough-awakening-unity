using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance; // Global access

    [Header("References")]
    public Transform weaponHolder; // Empty GameObject where weapons spawn (assign in Inspector)
    public GameObject fistPrefab;  // Fist model (assign in Inspector)

    [HideInInspector] public ItemSO currentItem; // Currently equipped item
    private GameObject currentWeapon; // Instantiated weapon/fist model

    void Awake()
    {
        instance = this;
        EquipFist(); // Start with fists
    }

    public void EquipItem(ItemSO newItem)
    {
        DestroyCurrentWeapon();
        currentItem = newItem;

        // Equip fists or new weapon
        if (newItem == Inventory.instance.fistItem)
        {
            EquipFist();
        }
        else
        {
            EquipNewWeapon(newItem);
        }
    }

    void EquipFist()
    {
        currentWeapon = Instantiate(fistPrefab, weaponHolder);
    }

    void EquipNewWeapon(ItemSO item)
    {
        DestroyCurrentWeapon(); //empeche duplicate weapons installed
        currentWeapon = Instantiate(item.prefab, weaponHolder);
        PositionWeapon();
    }

    void DestroyCurrentWeapon()
    {
        if (currentWeapon != null) 
            Destroy(currentWeapon);
    }

    void PositionWeapon()
    {
     
        currentWeapon.transform.localPosition = new Vector3(0.572f, -0.005f, -0.002f);

    }
}