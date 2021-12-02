using UnityEngine;

public class InventoryUI : MonoBehaviour {
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private ItemSlot inventorySlotPrefab;
    [SerializeField] private Item inventoryItemPrefab;
    private Inventory inventory; 
    private void Start() { 
        inventory = Inventory.Instance;  
        inventory.OnInventoryUpdated += Redraw;
        Redraw(); 
    }

    private void Redraw() {
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