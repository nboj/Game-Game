using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : AggressiveCreature {
    [Header("Player Controls")] [SerializeField]
    private Canvas playerInventoryUI;
    [SerializeField] private UISlotsController UIController;
    [SerializeField] private Slider[] reloadSliders;
    [SerializeField] private Slider selectedReloadSlider; 
    [SerializeField] private Weapon_SO defaultWeapon;
    private Inventory inventory;

    public Inventory Inventory => inventory;
    public override void Awake() {
        base.Awake();
        inventory = GetComponent<Inventory>();
    }

    public override void Start() { 
        base.Start();
        var maxIndex = UIController.LeftMaxIndex;
        // Adds the extra slots if they are empty
        #region Adds any extra slots if they were empty
        if (Weapons.Count < maxIndex) {
            for (int i = 0; i <= ((1 + maxIndex) - Weapons.Count); i++) {
                Weapons.Add(defaultWeapon);
            }
        }
        #endregion
        
        // Initializes each weapon slot with a sprite
        #region Initialise each equipped slot with a sprite
        for (int i = 0; i <= maxIndex; i++) {
            if (Weapons[i] != null)
                UIController.SetLeftSlot(i, Weapons[i].WeaponSprite);
            else
                UIController.SetLeftSlot(i, defaultWeapon.WeaponSprite);
        }
        #endregion
        
        // Sets max values for all of the reload sliders
        #region Loop through reload sliders
        for (int i = 0; i < Weapons.Count; i++) {
            var maxAmount = Weapons[i].FireRate;
            if (maxAmount == 0)
                maxAmount = 0.01f;
            reloadSliders[i].maxValue = maxAmount;
        }
        var max = Weapons[SelectedIndex].FireRate;
        if (max == 0)
            max = 0.01f;
        selectedReloadSlider.maxValue = max;
        #endregion
        UpdateReloadTimes();
    }

    public override void Update() {
        PointToMouse();
    }

    public override void FixedUpdate() {
        RigidbodyMovement.FixedUpdate();  
        UpdateReloadSliders(); 
    }

    #region Player Inputs
    public void OnMove(InputValue value) {
        if (CanControl) {
            var direction = value.Get<Vector2>();
            RigidbodyMovement.SetDirection(direction);
        }
    }

    private void OnTab() {
        playerInventoryUI.enabled = !playerInventoryUI.enabled;
    }

    private void OnQ() {
        var maxIndex = UIController.LeftMaxIndex;
        int index = UIController.LeftSelectedIndex > 0 ? UIController.LeftSelectedIndex - 1 : maxIndex;
        SetSelectedIndex(index);
    }

    private void OnE() {
        var maxIndex = UIController.LeftMaxIndex;
        int index = UIController.LeftSelectedIndex < maxIndex ? UIController.LeftSelectedIndex + 1 : 0;
        SetSelectedIndex(index);
    }

    private void OnFire() {
        if (CanAttack) {
            var target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Fire(target); 
        }
    }

    private void OnF() {
        var hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Portal"));
        if (hit != null) {
            var portal = hit.GetComponent<Portal>();
            portal.Teleport();
        }
    }
    #endregion

    private new void SetSelectedIndex(int index) { 
        UIController.SetLeftSelectedSlot(index);
        base.SetSelectedIndex(index);
        var maxAmount = Weapons[SelectedIndex].FireRate;
        if (maxAmount == 0)
            maxAmount = 0.01f;
        selectedReloadSlider.maxValue = maxAmount;
    }

    private void UpdateReloadSliders() {
        for (int i = 0; i < Weapons.Count; i++) {
            var setAmount = Time.time - ReloadDelays[i];
            reloadSliders[i].value = setAmount;
        }
        selectedReloadSlider.value = Time.time - ReloadDelays[SelectedIndex];
    }

    private void PointToMouse() {
        var direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        var velocityDirection = RigidbodyMovement.Direction;
        if (velocityDirection.x > 0 || velocityDirection.y > 0 || velocityDirection.x < 0 || velocityDirection.y < 0) {
            Animator.SetBool("isIdleDown", false);
            Animator.SetBool("isIdleUp", false);
            if (direction.y < 0 && direction.y < Mathf.Abs(direction.x) &&
                direction.y < -Mathf.Abs(direction.x)) {
                transform.localScale = new Vector3(1, 1, 1);
                Animator.SetBool("isWalking", false);
                Animator.SetBool("isWalkingUp", false);
                Animator.SetBool("isWalkingDown", true);
            } else if (direction.y > 0 && direction.y > Mathf.Abs(direction.x) &&
                       direction.y > -Mathf.Abs(direction.x)) {
                transform.localScale = new Vector3(1, 1, 1);
                Animator.SetBool("isWalking", false);
                Animator.SetBool("isWalkingDown", false);
                Animator.SetBool("isWalkingUp", true);
            } else if (direction.x < 0) {
                transform.localScale = new Vector3(-1, 1, 1);
                Animator.SetBool("isWalkingDown", false);
                Animator.SetBool("isWalkingUp", false);
                Animator.SetBool("isWalking", true);
            } else if (direction.x >= 0) {
                transform.localScale = new Vector3(1, 1, 1);
                Animator.SetBool("isWalkingDown", false);
                Animator.SetBool("isWalkingUp", false);
                Animator.SetBool("isWalking", true);
            }
        } else {
            Animator.SetBool("isWalkingDown", false);
            Animator.SetBool("isWalking", false);
            Animator.SetBool("isWalkingUp", false);
            if (direction.y < 0 && direction.y < Mathf.Abs(direction.x) &&
                direction.y < -Mathf.Abs(direction.x)) {
                Animator.SetBool("isIdleDown", true);
                Animator.SetBool("isIdleUp", false);
                transform.localScale = new Vector3(1, 1, 1);
            } else if (direction.y > 0 && direction.y > Mathf.Abs(direction.x) &&
                       direction.y > -Mathf.Abs(direction.x)) {
                Animator.SetBool("isIdleDown", false);
                Animator.SetBool("isIdle", false);
                Animator.SetBool("isIdleUp", true);
            } else if (direction.x < 0) {
                Animator.SetBool("isIdleDown", false);
                Animator.SetBool("isIdleUp", false);
                transform.localScale = new Vector3(-1, 1, 1);
            } else if (direction.x >= 0) {
                Animator.SetBool("isIdleDown", false);
                Animator.SetBool("isIdleUp", false);
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}