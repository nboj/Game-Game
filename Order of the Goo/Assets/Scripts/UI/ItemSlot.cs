using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDragContainer<DragItem>, IDropHandler {

    public void AddItem(DragItem item) {
        item.transform.SetParent(transform);
        item.transform.position = transform.position;
        item.OriginalParent = transform;
    }

    public void RemoveItem() { 
    }

    public void OnDrop(PointerEventData eventData) { 
        var otherItem = eventData.pointerDrag;
        Transform currentItem = null;
        try {
            currentItem = transform.GetChild(0); 
        } catch(UnityException) { 
        }
        if (currentItem != null) {  
            var otherItemParent = otherItem.GetComponent<DragItem>().OriginalParent;
            var destination = otherItemParent.GetComponent<ItemSlot>(); 
            destination.AddItem(currentItem.GetComponent<Item>());
        }
        AddItem(otherItem.GetComponent<Item>());
    }
} 