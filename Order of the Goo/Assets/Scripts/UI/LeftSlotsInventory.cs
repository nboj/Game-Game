using UnityEngine;


public delegate void WeaponSlotsUpdated();
public class LeftSlotsInventory : Inventory {
    public event WeaponSlotsUpdated OnWeaponSlotsUpdated; 
    public static new LeftSlotsInventory Instance {
        get { return GameObject.FindWithTag("Player").GetComponent<Player>().LeftSlotsInventory; }
    }

    protected override void Awake() {
        base.Awake();
        var player = FindObjectOfType<Player>(); 
        var index = 0;
        while (index < entities.Length && index < player.Weapons.Count) { 
            entities[index] = player.Weapons[index];
            index++;
        }
    }

    public override void AddItem(Entity entity, int index) {
        base.AddItem(entity, index);
        UpdateSlots();
    }

    public override void RemoveItem(int index) {
        base.RemoveItem(index);
        UpdateSlots();
    }

    public override Entity GetEntity(int index) {
        var entity = base.GetEntity(index);
        UpdateSlots();
        return entity;
    }

    public override void SwapItems(int calledIndex, int otherIndex, Inventory otherInventory) {
        base.SwapItems(calledIndex, otherIndex, otherInventory);
        UpdateSlots();
    }

    private void UpdateSlots() {
        Debug.Log(Entities.Length);
        if (OnWeaponSlotsUpdated != null) {
            OnWeaponSlotsUpdated();
        }
    }
}
