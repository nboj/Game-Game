using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        var dataObj = eventData.pointerDrag.gameObject;
        var dataObjItem = dataObj.GetComponent<Item>();
        dataObj.transform.SetParent(transform);
        dataObj.transform.position = transform.position;
        var item = transform.GetComponentInChildren<Item>();
        item.transform.SetParent(dataObjItem.OriginalParent);
        item.transform.position = dataObjItem.OriginalParent.transform.position;
        Debug.Log("Dropped");
    }
}