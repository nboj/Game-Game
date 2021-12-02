using UnityEngine;

public delegate void InventoryUpdated();
public class Inventory : MonoBehaviour {
    [SerializeField] int inventorySize = 32;
    private Entity[] entities;
    public Entity[] Entities => entities;
    public event InventoryUpdated OnInventoryUpdated;
    public static Inventory Instance => GameObject.FindWithTag("Player").GetComponent<Player>().Inventory;
    // Add item
    // Swap item--------- In add item, check if null and use that to determine swap
    // Update UI
    private void Awake() {
        entities = new Entity[inventorySize];
        var player = FindObjectOfType<Player>();
        for (var i = 0; i < player.Weapons.Count; i++) {
            entities[i] = player.Weapons[i]; 
        }
    }

    private void Start() {
        UpdateUI();
    }

    public void AddItem(Entity entity, int index) {
        entities[index] = entity;
    } 

    public void RemoveItem(int index) {
        entities[index] = null;
    }

    public Entity GetEntity(int index) {
        return entities[index];
    }

    public void UpdateUI() {
        if (OnInventoryUpdated != null) {
            OnInventoryUpdated(); 
        }
    }
}
