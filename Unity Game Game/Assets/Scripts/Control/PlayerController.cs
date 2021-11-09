using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector; 
using RPG.Combat;
using RPG.Core;
using UnityEngine.UI;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
    [BoxGroup("Player Controls", centerLabel:true)]
    [LabelText("Player Speed")]
    [LabelWidth(100)]
    [Required]
    [SerializeField] float _playerSpeed = 10;
    [BoxGroup("Player Controls", centerLabel:true)]
    [LabelText("UI Slots")]
    [LabelWidth(100)]
    [Required]
    [SerializeField] Image[] itemSlots; 
    [BoxGroup("Player Controls", centerLabel:true)]
    [LabelText("Player Weapons")]
    [LabelWidth(100)]
    [Required]
    [SerializeField] WeaponSO[] weapons;
    private Rigidbody2D _playerRigidbody;
    private Vector2 _playerVelocity;
    private Animator _playerAnimator; 
    private int _selectedWeaponIndex;
    private Fighter _playerFighter;
    private Color _originalSlotColor;
    private Color _selectedSlotColor = Color.white;
    public int WeaponsArrayLength { get => weapons.Length; }
    public int SelectedIndex { get => _selectedWeaponIndex; }
    private void Start() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>(); 
        _playerFighter = GetComponent<Fighter>(); 
        Image panel = itemSlots[_selectedWeaponIndex];
        _originalSlotColor = panel.color;
        panel.color = _selectedSlotColor;
    }
 
    private void Update() {
        _playerRigidbody.velocity = _playerVelocity;
    }

    private void OnMove(InputValue value) { 
        _playerVelocity = GetPlayerVelocity(value);
    }

    private void OnFire() {
        _playerFighter.FireWeapon(weapons[_selectedWeaponIndex]);
    }

    private void OnButton1() {
        SetSelectedSlot(0);
    }

    private void OnButton2() { 
        SetSelectedSlot(1);
    }
 
    private void SetSelectedSlot(int slotIndex) { 
        Image prePanel = itemSlots[_selectedWeaponIndex];
        if(_selectedWeaponIndex == slotIndex) return; 
        prePanel.color = _originalSlotColor;
        _selectedWeaponIndex = slotIndex; 
        Image postPanel = itemSlots[_selectedWeaponIndex];
        postPanel.color = _selectedSlotColor;
    }

    private Vector2 GetPlayerVelocity(InputValue value) {
        Vector2 inputAxis = value.Get<Vector2>();
        bool isHorizontal = Mathf.Abs(inputAxis.x) > Mathf.Epsilon;
        bool isVertical = Mathf.Abs(inputAxis.y) > Mathf.Epsilon;
        if (isHorizontal) {
            transform.localScale = new Vector3(Mathf.Sign(inputAxis.x), 1, 1);
            _playerAnimator.SetBool("isWalking", true);
        } else {
            _playerAnimator.SetBool("isWalking", false);
        }
        return inputAxis * _playerSpeed;
    }
}

}