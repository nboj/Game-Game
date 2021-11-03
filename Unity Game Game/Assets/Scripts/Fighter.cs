using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Fighter : MonoBehaviour { 
    [SerializeField] Weapon[] _weapons;
    [SerializeField] Image[] _weaponSlots;
    [SerializeField] int _selectedWeaponIndex;
    [SerializeField] Slider[] _slotSliders;
    private Color _originalSlotColor;
    private Color _selectedSlotColor = Color.white;

    void Start() {
        _originalSlotColor = _weaponSlots[0].color;
        _weaponSlots[0].color = _selectedSlotColor;
    }
 
    void Update() {
        
    }

    private void OnFire() {
        _weapons[_selectedWeaponIndex].FireWeapon(transform, _slotSliders[_selectedWeaponIndex]);
    }
 
    private void OnButton1() {
        SetSelectedSlot(0);
    }

    private void OnButton2() {
        SetSelectedSlot(1);
    }

    private void SetSelectedSlot(int slotIndex) { 
        if(_selectedWeaponIndex == slotIndex) return;
        _weaponSlots[_selectedWeaponIndex].color = _originalSlotColor;
        _selectedWeaponIndex = slotIndex;
        _weaponSlots[_selectedWeaponIndex].color = _selectedSlotColor;
    }
}
