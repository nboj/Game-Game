using UnityEngine;
using System.Collections;
using RPG.Saving;
using System.Collections.Generic;

public delegate void InventoryUpdated();
public class Inventory : MonoBehaviour, ISaveable {
    [SerializeField] int inventorySize = 32; 
    protected Entity[] entities; 
    public Entity[] Entities => entities;
    public event InventoryUpdated OnInventoryUpdated;
    public static Inventory Instance => GameObject.FindWithTag("Player").GetComponent<Player>().PlayerInventory; 
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

    //[System.Serializable]
    //struct SavableEntity {
    //    public string id;
    //    public bool canPickup;
    //    public bool canDrop;
    //    public float pickupRadius;
    //    public string description;
    //    public float weight;
    //    public Texture2D sprite;

    //    public SavableEntity(string id, bool canPickup, bool canDrop, float pickupRadius, string description, float weight, Sprite sprite) {
    //        this.id = id;
    //        this.canDrop = canDrop; 
    //        this.canPickup = canPickup;
    //        this.pickupRadius = pickupRadius;
    //        this.description = description;
    //        this.weight = weight;
    //        this.sprite = sprite.texture;
    //    }
    //}

    public Entity GetFromID(string id) {
        var itemList = Resources.LoadAll<Entity>("");
        foreach (var item in itemList) {
            if (id.Equals(item.ID)) {
                return item;
            }
        }
        return null;
    }

    object ISaveable.CaptureState() {
        //SavableEntity[] savableEntities = new SavableEntity[entities.Length];
        //for (var i = 0; i < entities.Length; i++) { 
        //    var entity = entities[i];
        //    if (entity != null) {
        //        SavableEntity newEntity = new SavableEntity(entity.ID, entity.CanPickup, entity.CanDrop, entity.PickupRadius, entity.Description, entity.Weight, entity.Sprite);
        //        savableEntities[i] = newEntity;
        //    }
        //}
        //return savableEntities;
        Dictionary<string, int> items = new Dictionary<string, int>();
        for (var i = 0; i < entities.Length; i++) {
            if (entities[i] != null) { 
                items.Add(entities[i].ID, i);
            }
        }
        return items;
    }

    void ISaveable.RestoreState(object state) {  
        var items = (Dictionary<string, int>)state;
        foreach (var item in items) {
            entities[item.Value] = GetFromID(item.Key); 
        }
    }
}
