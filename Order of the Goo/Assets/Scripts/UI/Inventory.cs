using UnityEngine;

public delegate void InventoryUpdated();
public class Inventory : MonoBehaviour {
    [SerializeField] int inventorySize = 32; 
    protected Entity[] entities; 
    public Entity[] Entities => entities;
    public event InventoryUpdated OnInventoryUpdated;
    public static Inventory Instance => GameObject.FindWithTag("Player").GetComponent<Player>().Inventory; 
    protected virtual void Awake() {
        entities = new Entity[inventorySize]; 
    }

    private void Start() {
        var player = FindObjectOfType<Player>(); 
        UpdateUI();
    }

    public virtual void AddItem(Entity entity, int index) {
        entities[index] = entity;
    } 

    public virtual void RemoveItem(int index) {
        entities[index] = null;
    }

    public virtual Entity GetEntity(int index) { 
        return entities[index];
    }

    public virtual void SwapItems(int calledIndex, int otherIndex, Inventory otherInventory) {
        var otherEntity = otherInventory.entities[otherIndex];
        var calledEntity = entities[calledIndex];
        otherInventory.entities[otherIndex] = calledEntity;
        entities[calledIndex] = otherEntity;
    }

    public void UpdateUI() {
        if (OnInventoryUpdated != null) {
            OnInventoryUpdated(); 
        }
    } 
}
