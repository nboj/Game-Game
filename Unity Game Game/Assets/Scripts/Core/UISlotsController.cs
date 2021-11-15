using UnityEngine;
using UnityEngine.UI;

public class UISlotsController : MonoBehaviour {
    [SerializeField] Sprite emptySlot;
    [SerializeField] Image rightSelectedSlot;
    [SerializeField] Image leftSelectedSlot;
    [SerializeField] Image[] rightSlots;
    [SerializeField] Image[] leftSlots;
    private int leftSelectedIndex;
    private int rightSelectedIndex;

    private void Start() {
        leftSelectedIndex = 0;
        rightSelectedIndex = 0;
        rightSelectedSlot.sprite = emptySlot;
        leftSelectedSlot.sprite = emptySlot;
        for (int i = 0; i < rightSlots.Length; i++) {
            rightSlots[i].sprite = emptySlot;
            leftSlots[i].sprite = emptySlot;
        }
    }

    public void SetLeftSelectedSlot(int slotIndex) {
        leftSelectedIndex = slotIndex;
        leftSelectedSlot.sprite = leftSlots[slotIndex].sprite;
    }

    public void SetRightSelectedSlot(int slotIndex) {
        rightSelectedIndex = slotIndex;
        rightSelectedSlot.sprite = rightSlots[slotIndex].sprite;
    }

    public void SetLeftSlot(int index, Sprite sprite) {
        leftSlots[index].sprite = sprite;
        if (leftSelectedIndex == index) {
            leftSelectedSlot.sprite = leftSlots[index].sprite;
        }
    }
    
    public void SetRightSlot(int index, Sprite sprite) {
        rightSlots[index].sprite = sprite;
        if (rightSelectedIndex == index) {
            rightSelectedSlot.sprite = rightSlots[index].sprite;
        }
    }
}