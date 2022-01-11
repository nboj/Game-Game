using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using RPG.UI;
using RPG.Dialogue;
using RPG.Saving;

public interface IFHandler {
    public void Fire();
} 
 
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(Collider2D)), RequireComponent(typeof(Inventory), typeof(LeftSlotsInventory))]
public class Player : AggressiveCreature, ISaveable { 
    [Header("Player Controls")] [SerializeField]
    private Canvas playerInventoryUI;
    [SerializeField] private UISlotsController UIController;
    [SerializeField] private Slider[] reloadSliders;
    [SerializeField] private Slider selectedReloadSlider; 
    [SerializeField] private Weapon_SO defaultWeapon;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private BoxCollider2D feetCollider; 
    private bool tempCanAttack = false;
    private Inventory inventory;
    private LeftSlotsInventory leftSlotsInventory; 
    public Inventory PlayerInventory => inventory;
    public LeftSlotsInventory LeftSlotsInventory => leftSlotsInventory;
    private DialogueConversant dialogueConversant;
    public override bool CanAttack {
        get => base.CanAttack;
        set {
            if (value) {
                UIController.CanvasEnabled(true);
            } else {
                UIController.CanvasEnabled(false); 
            } 
            base.CanAttack = value;
        }
    }
    public override void Awake() {
        CanAttack = CanAttack; 
        base.Awake();
        inventory = GetComponent<Inventory>();
        leftSlotsInventory = GetComponent<LeftSlotsInventory>();
        dialogueConversant = GetComponent<DialogueConversant>();
    }


    public override void Start() { 
        base.Start();
        
        leftSlotsInventory.OnWeaponSlotsUpdated += UpdateWeapons;
        UpdateUI();
    }

    public DialogueConversant DialogueConversant {
        get => dialogueConversant;
    }
    
    public void UpdateWeapons() {
        var entities = leftSlotsInventory.Entities;
        Weapons.Clear();
        foreach (Entity entity in entities) { 
            if (entity == null)
                Weapons.Add(defaultWeapon);
            else
                Weapons.Add((Weapon_SO)entity);
        }
        UpdateUI();
    }

    public void UpdateUI() { 
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
                UIController.SetLeftSlot(i, Weapons[i].Sprite);
            else
                UIController.SetLeftSlot(i, defaultWeapon.Sprite);
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
        if (CanAttack)
            PointToMouse();
        else
            RigidbodyMovement.SetAnimator(RigidbodyMovement.Direction);
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
        var hit = Physics2D.OverlapBox((Vector2)feetCollider.transform.position + feetCollider.offset, feetCollider.size, 0, LayerMask.GetMask("FTrigger"));
        if (hit != null) {
            hit.gameObject.GetComponent<IFHandler>().Fire(); 
        }
    }

    private void OnSpace() {  
        if (!dialogueConversant.HasNext()) {
            Enable();
        }
        if (dialogueUI.Canvas.enabled) {
            dialogueUI.Next(); 
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

    public void Disable() {
        tempCanAttack = CanAttack;
        CanControl = false;
        CanAttack = false;
        RigidbodyMovement.CanControl = false;
    }

    public void Enable() {
        CanControl = true;
        CanAttack = tempCanAttack;
        RigidbodyMovement.CanControl = true;

    }

    public void Enable(bool CanControl = true, bool CanAttack = true) {
        this.CanControl = CanControl;
        this.CanAttack = CanAttack;
        RigidbodyMovement.CanControl = CanControl; 
    }

    private void PointToMouse() {
        var direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        var velocityDirection = RigidbodyMovement.Direction;
        if (velocityDirection.x > 0 || velocityDirection.y > 0 || velocityDirection.x < 0 || velocityDirection.y < 0) {
            Animator.SetBool("Idle Down", false);
            Animator.SetBool("Idle Up", false);
            Animator.SetBool("Idle Right", false);
            Animator.SetBool("Idle Left", false);
            if (direction.y < 0 && direction.y < Mathf.Abs(direction.x) &&
                direction.y < -Mathf.Abs(direction.x)) { 
                Animator.SetBool("Right", false);
                Animator.SetBool("Up", false);
                Animator.SetBool("Down", true);
                Animator.SetBool("Left", false);
            } else if (direction.y > 0 && direction.y > Mathf.Abs(direction.x) &&
                       direction.y > -Mathf.Abs(direction.x)) { 
                Animator.SetBool("Right", false);
                Animator.SetBool("Down", false);
                Animator.SetBool("Up", true);
                Animator.SetBool("Left", false);
            } else if (direction.x < 0) { 
                Animator.SetBool("Down", false);
                Animator.SetBool("Up", false);
                Animator.SetBool("Right", false);
                Animator.SetBool("Left", true);
            } else if (direction.x >= 0) { 
                Animator.SetBool("Down", false);
                Animator.SetBool("Up", false);
                Animator.SetBool("Right", true);
                Animator.SetBool("Left", false);
            }
        } else {
            Animator.SetBool("Down", false);
            Animator.SetBool("Right", false);
            Animator.SetBool("Up", false);
            Animator.SetBool("Left", false);
            if (direction.y < 0 && direction.y < Mathf.Abs(direction.x) &&
                direction.y < -Mathf.Abs(direction.x)) {
                Animator.SetBool("Idle Down", true);
                Animator.SetBool("Idle Up", false);
                Animator.SetBool("Idle Right", false);
                Animator.SetBool("Idle Left", false);
                transform.localScale = new Vector3(1, 1, 1);
            } else if (direction.y > 0 && direction.y > Mathf.Abs(direction.x) &&
                       direction.y > -Mathf.Abs(direction.x)) {
                Animator.SetBool("Idle Down", false);
                Animator.SetBool("Idle Right", false);
                Animator.SetBool("Idle Left", false);
                Animator.SetBool("Idle Up", true);
            } else if (direction.x < 0) {
                Animator.SetBool("Idle Down", false);
                Animator.SetBool("Idle Up", false);
                Animator.SetBool("Idle Right", false);
                Animator.SetBool("Idle Left", true); 
            } else if (direction.x >= 0) {
                Animator.SetBool("Idle Down", false);
                Animator.SetBool("Idle Up", false);
                Animator.SetBool("Idle Right", true);
                Animator.SetBool("Idle Left", false); 
            }
        }
    }

    object ISaveable.CaptureState() {
        return new SerializableVector3(transform.position);
    }

    void ISaveable.RestoreState(object state) {
        transform.position = ((SerializableVector3)state).ToVector();
    }
}