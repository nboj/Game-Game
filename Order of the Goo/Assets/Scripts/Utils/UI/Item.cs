using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler {
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    
    public Transform OriginalParent { get => originalParent; }

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        SetOriginalParent();
    }
    
    public void OnDrag(PointerEventData eventData) {
        var mousePos = Mouse.current.position.ReadValue();
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

        canvasGroup.blocksRaycasts = false;
        
        Debug.Log("Drag" + mousePos);
    } 

    public void OnPointerDown(PointerEventData eventData) {
        SetOriginalParent();
        transform.parent = originalParent.parent;
        Debug.Log("Clicked");
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == originalParent) {
            transform.position = originalParent.position;
        }
        
        Debug.Log("End Drag/dropped");
    }

    private void SetOriginalParent() {
        originalParent = transform.parent;
    }
}