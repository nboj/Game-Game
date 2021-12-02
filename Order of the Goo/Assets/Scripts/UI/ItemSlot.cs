using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDragContainer<DragItem>, IDropHandler {
    private int index;
    private Inventory inventory;

    public void AddItem(DragItem item) {
        item.transform.SetParent(transform);
        item.transform.position = transform.position;
        item.OriginalParent = transform;
    }

    public void RemoveItem() { 
    }

    public void Setup(Inventory inventory, int index, Item item) {
        this.inventory = inventory;
        this.index = index;
        var entity = inventory.GetEntity(index);
        if (entity != null) {
            Debug.Log(entity as Weapon_SO);
            var itemOb = Instantiate(item, transform);
            itemOb.Entity = entity;
            itemOb.GetComponent<Image>().sprite = item.Entity.Sprite;
        }
    }

    public void OnDrop(PointerEventData eventData) { 
        var otherItem = eventData.pointerDrag;
        Transform currentItem = null;
        try {
            currentItem = transform.GetChild(0); 
        } catch(UnityException) { 
        }
        var otherItemParent = otherItem.GetComponent<DragItem>().OriginalParent;
        var destination = otherItemParent.GetComponent<ItemSlot>(); 
        AddItem(otherItem.GetComponent<Item>());
        if (currentItem != null) {  
            destination.AddItem(currentItem.GetComponent<Item>());
            // Call swap items
        } else {
            inventory.RemoveItem(destination.index);
            inventory.AddItem(otherItem.GetComponent<Item>().Entity , index);
            // Call set item
        }
        inventory.UpdateUI();
        // Call UpdateUI in inventory
    }
} 