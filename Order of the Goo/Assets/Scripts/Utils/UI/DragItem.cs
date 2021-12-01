using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem<T> : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler where T : class {
    private Vector2 startPos;
    private Transform originalParent;

    private void Start() { 
        originalParent = transform.parent;
        startPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.parent = null;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    } 

    public void OnEndDrag(PointerEventData eventData) {
        startPos = transform.position;
        if (transform.parent == null) {
            transform.position = startPos;
        } else
            originalParent = transform.parent; // Finish this
    }
} 
