using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector; 
using RPG.Combat;
using RPG.Core;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        private enum LastMovementState {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
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
        private BoxCollider2D boxCollider;
        private Rigidbody2D _playerRigidbody;
        private Vector2 _playerVelocity;
        private Animator _playerAnimator; 
        private int _selectedWeaponIndex;
        private Fighter _playerFighter;
        private Color _originalSlotColor;
        private Color _selectedSlotColor = Color.white;
        private Vector3 playerDirection;   
        private bool canAttack;
        private bool canControl;
        private LastMovementState lastMovementState;
        private int lastSceneIndex;
        public int WeaponsArrayLength { get => weapons.Length; }
        public int SelectedIndex { get => _selectedWeaponIndex; }
        public bool CanAttack { get => canAttack; set => canAttack = value; }
        public bool CanControl { get => canControl; set => canControl = value; }
        
        private void Start() {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>(); 
            _playerFighter = GetComponent<Fighter>(); 
            boxCollider = GetComponent<BoxCollider2D>();
            Image panel = itemSlots[_selectedWeaponIndex];
            _originalSlotColor = panel.color;
            panel.color = _selectedSlotColor;  
            canAttack = false;
            canControl = true;
        }
    
        private void Update() { 
            if (canControl) {
                if (canAttack) {
                    PointToMouse();
                } else {
                    MoveNormal();
                }
                MovePlayer();
            }
        }

        private void MovePlayer() {  
            _playerRigidbody.velocity = _playerVelocity;
        }

        private void MoveNormal() { 
            bool isLeft = _playerVelocity.x < 0;
            bool isRight = _playerVelocity.x > 0;
            bool isUp = _playerVelocity.y > 0;
            bool isDown = _playerVelocity.y < 0; 
            bool moving = isLeft || isRight || isUp || isDown;
            if (moving) {
                _playerAnimator.SetBool("isIdle", false); 
                _playerAnimator.SetBool("isIdleDown", false);
                if (isLeft) {
                    _playerAnimator.SetBool("isWalkingDown", false);  
                    _playerAnimator.SetBool("isWalking", true); 
                    transform.localScale = new Vector3(-1, 1, 1);
                    lastMovementState = LastMovementState.LEFT;
                } else if (isRight) {
                    _playerAnimator.SetBool("isWalkingDown", false);  
                    _playerAnimator.SetBool("isWalking", true); 
                    transform.localScale = new Vector3(1, 1, 1); 
                    lastMovementState = LastMovementState.RIGHT;
                } else if (isUp) {  
                    _playerAnimator.SetBool("isWalkingDown", false); 
                    _playerAnimator.SetBool("isWalking", true); 
                    transform.localScale = new Vector3(1, 1, 1);
                    lastMovementState = LastMovementState.UP;
                } else if (isDown) { 
                    _playerAnimator.SetBool("isWalking", false);  
                    _playerAnimator.SetBool("isWalkingDown", true); 
                    transform.localScale = new Vector3(1, 1, 1);
                    lastMovementState = LastMovementState.DOWN;
                }
            } else {
                _playerAnimator.SetBool("isWalking", false);  
                _playerAnimator.SetBool("isWalkingDown", false); 
                switch (lastMovementState) {
                    case LastMovementState.UP:
                        _playerAnimator.SetBool("isIdle", true); 
                        break;
                    case LastMovementState.DOWN:
                        _playerAnimator.SetBool("isIdleDown", true);
                        break;
                    case LastMovementState.RIGHT:
                    case LastMovementState.LEFT:
                        _playerAnimator.SetBool("isIdle", true);
                        break;  
                }
            }
        }

        private void PointToMouse() { 
            playerDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;  
            if (_playerVelocity.x > 0 || _playerVelocity.y > 0 || _playerVelocity.x < 0 || _playerVelocity.y < 0) { 
                _playerAnimator.SetBool("isIdleDown", false);   
            if (playerDirection.y < 0 && playerDirection.y < Mathf.Abs(playerDirection.x) && playerDirection.y < -Mathf.Abs(playerDirection.x)) {  
                    transform.localScale = new Vector3(1, 1, 1);
                    _playerAnimator.SetBool("isWalking", false);
                    _playerAnimator.SetBool("isWalkingDown", true);
                } else if (playerDirection.x < 0) {  
                    transform.localScale = new Vector3(-1, 1, 1);
                    _playerAnimator.SetBool("isWalkingDown", false);
                    _playerAnimator.SetBool("isWalking", true);  
                } else if (playerDirection.x >= 0) { 
                    transform.localScale = new Vector3(1, 1, 1);
                    _playerAnimator.SetBool("isWalkingDown", false);
                    _playerAnimator.SetBool("isWalking", true);   
                }
            }  else {
                _playerAnimator.SetBool("isWalkingDown", false);
                _playerAnimator.SetBool("isWalking", false);   
                if (playerDirection.y < 0 && playerDirection.y < Mathf.Abs(playerDirection.x) && playerDirection.y < -Mathf.Abs(playerDirection.x)) { 
                    _playerAnimator.SetBool("isIdleDown", true);
                    transform.localScale = new Vector3(1, 1, 1); 
                } else if (playerDirection.x < 0) { 
                    _playerAnimator.SetBool("isIdleDown", false);
                    transform.localScale = new Vector3(-1, 1, 1);
                } else if (playerDirection.x >= 0) { 
                    _playerAnimator.SetBool("isIdleDown", false);
                    transform.localScale = new Vector3(1, 1, 1);
                }
            } 
        }

        private void OnMove(InputValue value) { 
            _playerVelocity = GetPlayerVelocity(value);
        }

        private void OnFire() {
            if (canAttack)
                _playerFighter.FireWeapon(weapons[_selectedWeaponIndex]);
        }

        private void OnButton1() {
            SetSelectedSlot(0);
        }

        private void OnButton2() { 
            SetSelectedSlot(1);
        }

        private void OnFButton() {
            if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Door"))) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f); 
                Door door = hit.collider.GetComponent<Door>();  
                lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
                door.Travel(); 
            }
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
            
            return inputAxis * _playerSpeed;
        }
    }

}