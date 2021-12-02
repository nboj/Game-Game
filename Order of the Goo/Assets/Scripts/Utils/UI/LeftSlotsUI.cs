using UnityEngine;

public class LeftSlotsUI : InventoryUI {
    protected override void Start() {
        inventory = LeftSlotsInventory.Instance;
        inventory.OnInventoryUpdated += Redraw;
        Redraw();
    }

    protected override void Redraw() { 
        Debug.Log("Redrawing");
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        for (var i = 0; i < inventory.Entities.Length; i++) {
            var slot = Instantiate(inventorySlotPrefab, transform);
            slot.Setup(inventory, i, inventoryItemPrefab, parentCanvas);
        }
    }
}