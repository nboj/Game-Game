using UnityEngine;

[CreateAssetMenu(menuName = "Magic Weapon_SO", fileName = "New Magic Weapon")]
public class MagicWeapon_SO : RangedWeapon_SO {
    [Header("Magic Properties")]
    [SerializeField] private bool rangedMagic;
    [SerializeField] private GameObject magicProjectile;
    [SerializeField] private bool trackTarget;
    [Range(0f, 10f)]
    [SerializeField] private float trackingSensitivity;
    [SerializeField] private float intelligence = 10f;

    public bool RangedMagic => rangedMagic;
    public GameObject MagicProjectile => magicProjectile;  
    public bool TrackTarget => trackTarget; 
    public float TrackingSensitivity => trackingSensitivity;
    public float Intelligence => intelligence;
}