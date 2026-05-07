using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Attack : MonoBehaviour
{
    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    [SerializeField] private Camera povCamera;

void Start()
{
    if (povCamera == null)
    {
        povCamera = Camera.main;
        if (povCamera == null)
            Debug.LogWarning("No camera found for Attack script.");
    }
}


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Suicide();
        }
    }

void PerformAttack()
{
    if (EquipmentManager.instance == null)
    {
        Debug.LogWarning("No EquipmentManager found.");
        return;
    }

    ItemSO currentItemSO = EquipmentManager.instance.currentItem;
    if (currentItemSO == null)
    {
        Debug.LogWarning("No item equipped.");
        return;
    }

    // Play attack sound
    if (currentItemSO.attackSound != null)
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.spatialBlend = 0; // 2D
        tempSource.playOnAwake = false;
        tempSource.clip = currentItemSO.attackSound;
        tempSource.pitch = Random.Range(0.95f, 1.05f);
        tempSource.Play();
        Destroy(tempSource, currentItemSO.attackSound.length);
    }

    // Raycast for damage or interaction
    if (Physics.Raycast(povCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, currentItemSO.range))
    {
        HandleHit(hit, currentItemSO.damage, currentItemSO.weaponType);
    }
}

    void ChopWood(RaycastHit hit)
    {
        DestroyObstacle invisWall = hit.collider.GetComponent<DestroyObstacle>(); //get component/instance off the invis wall
        invisWall.DestroyItem(); //trigger script got from it, destroys gameobj set in component
    }


    void HandleHit(RaycastHit hit, int damage, WeaponType weaponType)
    {

        if (weaponType == WeaponType.Axe && hit.collider.CompareTag("FirstObstacle")) //check si axe est equip et si FirstObstacle
        {
            ChopWood(hit); //cuz easy, prop drilling
        }

        Health health = hit.collider.GetComponent<Health>();
        if (health != null)
        {
            health.Damage(damage);
        }
    }

    void Suicide()
    {
        GetComponent<Health>().Kill();
    }
}
