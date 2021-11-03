using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fighter : MonoBehaviour { 
    [SerializeField] Weapon[] _weapons;
    [SerializeField] int _selectedWeapon;

    void Start() {
        
    }
 
    void Update() {
        
    }

    public void OnFire() {
        _weapons[_selectedWeapon].FireWeapon(transform);
    }
}
