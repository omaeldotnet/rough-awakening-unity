// Item.cs
using UnityEngine;

public enum WeaponType { Fists, Melee, Ranged, Axe }

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName; //name displayed in UI
    public Sprite icon; //icon shown in hotbar
    public GameObject prefab; //3d model when equipped
    public WeaponType weaponType;
    public int damage;
    public float range;
    public AudioClip attackSound;
}