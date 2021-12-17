using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDragContainer<DragItem>, IDropHandler {
    private int index;
    private Inventory inventory;

    public int Index => index;

    public void AddItem(DragItem item) {
        item.transform.SetParent(transform);
        item.transform.position = transform.position;
        item.OriginalParent = transform;
    }

    public void RemoveItem() { 
        inventory.RemoveItem(index);
    }

    public void Setup(Inventory inventory, int index, Item itemPrefab, Canvas canvas) {
        this.inventory = inventory;
        this.index = index;
        var entity = inventory.GetEntity(index);
        if (entity != null) { 
            var itemOb = Instantiate(itemPrefab, transform); 
            itemOb.Entity = entity;
            itemOb.GetComponent<Image>().sprite = itemOb.Entity.Sprite; 
            itemOb.ParentCanvas = canvas;
        }
    }

    public void OnDrop(PointerEventData eventData) { 
        var otherItem = eventData.pointerDrag;  
        Transform currentItem = null;
        Transform otherItemParent = null;
        ItemSlot destination = null;
        try {
            currentItem = transform.GetChild(0);
        } catch (UnityException) {
        }
        var temp = otherItem.GetComponent<DragItem>();
        if (temp != null) { 
            otherItemParent = temp.OriginalParent;
            destination = otherItemParent.GetComponent<ItemSlot>(); 
            if (otherItemParent != null) {
                AddItem(otherItem.GetComponent<Item>());
                if (currentItem != null) {
                    destination.AddItem(currentItem.GetComponent<Item>());
                    inventory.SwapItems(index, destination.Index, destination.inventory);
                    // Call swap items
                } else {
                    destination.RemoveItem();
                    inventory.AddItem(otherItem.GetComponent<Item>().Entity, index);
                    // Call set item 
                }
            }
        } 
            inventory.UpdateUI();
        // Call UpdateUI in inventory 
    }
} 