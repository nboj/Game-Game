using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {
    [SerializeField] Canvas parentCanvas; 
    private Transform originalParent;
    private CanvasGroup canvasGroup; 

    public Transform OriginalParent {
        get => originalParent;
        set => originalParent = value;
    }

    private void Start() { 
        originalParent = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();  
    }

    public void OnBeginDrag(PointerEventData eventData) { 
        transform.SetParent(parentCanvas.transform); 
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    } 

    public void OnEndDrag(PointerEventData eventData) { 
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == parentCanvas.transform) { 
            transform.position = originalParent.position;
            transform.SetParent(originalParent);
        } else {
            originalParent = transform.parent; 
        } 
    }
} 
 