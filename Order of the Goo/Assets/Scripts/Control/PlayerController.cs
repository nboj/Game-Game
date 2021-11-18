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
        [SerializeField] Image[] itemSlots;   
        [SerializeField] bool canAttack = false;
        private BoxCollider2D boxCollider; 
        private Vector2 _playerVelocity;
        private Animator _playerAnimator; 
        private int _selectedWeaponIndex;
        private Fighter _playerFighter;
        private Color _originalSlotColor;
        private Color _selectedSlotColor = Color.white;
        private Vector3 playerDirection;   
        private bool canControl;
        private LastMovementState lastMovementState;
        private int lastSceneIndex;
        private Vector2 oldPos; 
        private bool canTeleport = false;
        private UISlotsController uISlotsController;
        private Mover mover;
        public bool CanTeleport { get => canTeleport; set => canTeleport = value; } 
        public int SelectedIndex { get => _selectedWeaponIndex; }
        public bool CanAttack { get => canAttack; set => canAttack = value; }
        public bool CanControl { get => canControl; set => canControl = value; }
        public int LastScene { get => lastSceneIndex; } 
        public Vector2 OldPos { get => oldPos; set => oldPos = value; }
        
        private void Start() { 
            _playerAnimator = GetComponent<Animator>(); 
            _playerFighter = GetComponent<Fighter>(); 
            boxCollider = GetComponent<BoxCollider2D>();
            mover = GetComponent<Mover>();
            Image panel = itemSlots[_selectedWeaponIndex];
            _originalSlotColor = panel.color;
            panel.color = _selectedSlotColor;   
            canControl = true;
            lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
            oldPos = transform.position; 
            uISlotsController = FindObjectOfType<UISlotsController>(); 
            WeaponSO[] weapons = _playerFighter.Weapons;
            for (int i = 0; i < weapons.Length; i++) {
                uISlotsController.SetLeftSlot(i, weapons[i].Projectile.GetComponent<SpriteRenderer>().sprite);
            }
        }
    
        private void Update() { 
            if (canTeleport) {
                if (lastSceneIndex == SceneManager.GetActiveScene().buildIndex) {
                    canTeleport = false;
                    canTeleportToOldPos();
                }
            }
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
            mover.SetDirection(_playerVelocity);
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
                _playerAnimator.SetBool("isIdleUp", false);
                if (isLeft) {
                _playerAnimator.SetBool("isWalkingUp", false);
                    _playerAnimator.SetBool("isWalkingDown", false);  
                    _playerAnimator.SetBool("isWalking", true); 
                    transform.localScale = new Vector3(-1, 1, 1);
                    lastMovementState = LastMovementState.LEFT;
                } else if (isRight) {
                _playerAnimator.SetBool("isWalkingUp", false);
                    _playerAnimator.SetBool("isWalkingDown", false);  
                    _playerAnimator.SetBool("isWalking", true); 
                    transform.localScale = new Vector3(1, 1, 1); 
                    lastMovementState = LastMovementState.RIGHT;
                } else if (isUp) {  
                    _playerAnimator.SetBool("isWalkingDown", false); 
                    _playerAnimator.SetBool("isWalking", false); 
                    _playerAnimator.SetBool("isWalkingUp", true);
                    transform.localScale = new Vector3(1, 1, 1);
                    lastMovementState = LastMovementState.UP;
                } else if (isDown) { 
                _playerAnimator.SetBool("isWalkingUp", false);
                    _playerAnimator.SetBool("isWalking", false);  
                    _playerAnimator.SetBool("isWalkingDown", true); 
                    transform.localScale = new Vector3(1, 1, 1);
                    lastMovementState = LastMovementState.DOWN;
                }
            } else {
                _playerAnimator.SetBool("isWalking", false);  
                _playerAnimator.SetBool("isWalkingDown", false); 
                _playerAnimator.SetBool("isWalkingUp", false); 
                switch (lastMovementState) {
                    case LastMovementState.UP:
                        _playerAnimator.SetBool("isIdleUp", true); 
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
                _playerAnimator.SetBool("isIdleUp", false);   
                if (playerDirection.y < 0 && playerDirection.y < Mathf.Abs(playerDirection.x) && playerDirection.y < -Mathf.Abs(playerDirection.x)) {  
                    transform.localScale = new Vector3(1, 1, 1);
                    _playerAnimator.SetBool("isWalking", false);
                    _playerAnimator.SetBool("isWalkingUp", false);
                    _playerAnimator.SetBool("isWalkingDown", true);
                } else if (playerDirection.y > 0 && playerDirection.y > Mathf.Abs(playerDirection.x) && playerDirection.y > -Mathf.Abs(playerDirection.x)) {  
                    transform.localScale = new Vector3(1, 1, 1);
                    _playerAnimator.SetBool("isWalking", false);
                    _playerAnimator.SetBool("isWalkingDown", false);
                    _playerAnimator.SetBool("isWalkingUp", true);
                } else if (playerDirection.x < 0) {  
                    transform.localScale = new Vector3(-1, 1, 1);
                    _playerAnimator.SetBool("isWalkingDown", false);
                    _playerAnimator.SetBool("isWalkingUp", false);
                    _playerAnimator.SetBool("isWalking", true);  
                } else if (playerDirection.x >= 0) { 
                    transform.localScale = new Vector3(1, 1, 1);
                    _playerAnimator.SetBool("isWalkingDown", false);
                    _playerAnimator.SetBool("isWalkingUp", false);
                    _playerAnimator.SetBool("isWalking", true);   
                }
            }  else {
                _playerAnimator.SetBool("isWalkingDown", false);
                _playerAnimator.SetBool("isWalking", false);   
                _playerAnimator.SetBool("isWalkingUp", false);
                if (playerDirection.y < 0 && playerDirection.y < Mathf.Abs(playerDirection.x) && playerDirection.y < -Mathf.Abs(playerDirection.x)) { 
                    _playerAnimator.SetBool("isIdleDown", true);
                    _playerAnimator.SetBool("isIdleUp", false);
                    transform.localScale = new Vector3(1, 1, 1); 
                } else if (playerDirection.y > 0 && playerDirection.y > Mathf.Abs(playerDirection.x) && playerDirection.y > -Mathf.Abs(playerDirection.x)) { 
                    _playerAnimator.SetBool("isIdleDown", false);
                    _playerAnimator.SetBool("isIdle", false);   
                    _playerAnimator.SetBool("isIdleUp", true);
                }  else if (playerDirection.x < 0) { 
                    _playerAnimator.SetBool("isIdleDown", false); 
                    _playerAnimator.SetBool("isIdleUp", false);
                    transform.localScale = new Vector3(-1, 1, 1);
                } else if (playerDirection.x >= 0) { 
                    _playerAnimator.SetBool("isIdleDown", false);
                    _playerAnimator.SetBool("isIdleUp", false);
                    transform.localScale = new Vector3(1, 1, 1);
                }
            } 
        }

        private void OnMove(InputValue value) { 
            _playerVelocity = value.Get<Vector2>();
        }

        private void OnFire() {
            if (canAttack) { 
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                _playerFighter.FireWeapon(mousePos);
            }
        }

        private void OnButton1() {
            SetSelectedSlot(0);
            OnFire();
        }

        private void OnButton2() { 
            SetSelectedSlot(1);
            OnFire();
        }

        private void OnFButton() { 
            LayerMask doorMask = LayerMask.GetMask("Door"); 
            if (boxCollider.IsTouchingLayers(doorMask)) { 
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.zero), 1f,  doorMask); 
                Door door = hit.collider.gameObject.GetComponent<Door>();  
                int index = SceneManager.GetActiveScene().buildIndex; 
                Vector2 pos = transform.position; 
                door.Travel();  
                lastSceneIndex = index;  
            }
        }

        public void canTeleportToOldPos() {  
            transform.position = oldPos;  
        }
    
        private void SetSelectedSlot(int slotIndex) {
            _playerFighter.SelectedWeaponIndex = slotIndex;
            if(_selectedWeaponIndex == slotIndex) return;  
            _selectedWeaponIndex = slotIndex;  
            uISlotsController.SetLeftSelectedSlot(slotIndex);   
        } 
    }
}