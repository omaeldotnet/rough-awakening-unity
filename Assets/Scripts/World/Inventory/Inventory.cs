using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Global access

    [Header("UI")]
    public Image[] slots;          // 3 UI slot images (assign in Inspector)
    public Color32 selectedColor = new Color32(78, 128, 195, 255);    // Highlight color for selected slot
    public Color32 normalBackground = new Color32(255, 255, 255, 0);

    [Header("Items")]
    public ItemSO fistItem;          // Fist ScriptableObject (assign in Inspector)
    public List<ItemSO> items = new List<ItemSO>(); // Holds fist + 2 collected items

    private int selectedIndex = 0; // Currently selected slot (0 = fist)

void Awake()
{
    instance = this;
}

void Start()
{
    InitializeFist();
}


    void InitializeFist()
    {
        //items.Clear();
        items.Add(fistItem); // ajoute fist dans list item
        UpdateUI();
        EquipItem(0); // Auto-equip fists
    }

    void Update() => HandleHotbarInput();

    void HandleHotbarInput()
    {
        // 1 = Fist, 2 = Slot2, 3 = Slot3
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) EquipItem(3);
    }

    public bool AddItem(ItemSO newItem)
    {
        items.Add(newItem);
        UpdateUI();
        return true;
    }

void EquipItem(int slotIndex)
{
    if (slotIndex >= items.Count) return;

    selectedIndex = slotIndex;

    if (EquipmentManager.instance != null)
    {
        EquipmentManager.instance.EquipItem(items[slotIndex]);
    }
    else
    {
        Debug.LogWarning("EquipmentManager instance is null. Cannot equip item.");
    }

    UpdateUI();
}


    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool slotHasItem = i < items.Count; //pour ajouter en ordre
            slots[i].sprite = slotHasItem ? items[i].icon : null; //va rajouter item
            slots[i].color = slotHasItem ? Color.white : new Color(0, 0, 0, 0);

            // Highlight selected slot border
            slots[i].transform.parent.GetComponent<Image>().color =
                (i == selectedIndex) ? selectedColor : normalBackground;
        }
    }
}