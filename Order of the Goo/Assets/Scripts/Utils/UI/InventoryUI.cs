using UnityEngine;

public class InventoryUI : MonoBehaviour {
    [SerializeField] protected Canvas parentCanvas;
    [SerializeField] protected ItemSlot inventorySlotPrefab;
    [SerializeField] protected Item inventoryItemPrefab;
    protected Inventory inventory; 
    protected virtual void Start() { 
        inventory = Inventory.Instance;  
        inventory.OnInventoryUpdated += Redraw;
        Redraw(); 
    }

    protected virtual void Redraw() {
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