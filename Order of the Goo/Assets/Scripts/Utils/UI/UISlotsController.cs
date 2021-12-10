using UnityEngine;
using UnityEngine.UI;

public class UISlotsController : MonoBehaviour {
    [SerializeField] Image rightSelectedSlot;
    [SerializeField] Image leftSelectedSlot;
    [SerializeField] Image[] rightSlots;
    [SerializeField] Image[] leftSlots;
    [SerializeField] private Sprite defaultSlotSprite;
    private int leftSelectedIndex;
    private int rightSelectedIndex;
    private int leftMaxIndex;
    private int rightMaxIndex;

    public int LeftSelectedIndex => leftSelectedIndex;
    public int RightSelectedIndex => rightSelectedIndex;
    public int LeftMaxIndex => leftMaxIndex;
    public int RightMaxIndex => rightMaxIndex;

    private void Awake() {
        leftSelectedIndex = 0;
        rightSelectedIndex = 0;
        leftMaxIndex = leftSlots.Length - 1;
        rightMaxIndex = rightSlots.Length - 1;
        for (int i = 0; i <= leftMaxIndex; i++) {
            SetLeftSlot(i, defaultSlotSprite);
            leftSlots[i].enabled = false;
        }
        SetLeftSelectedSlot(leftSelectedIndex);
    }

    public void SetLeftSelectedSlot(int slotIndex) {
        leftSelectedIndex = slotIndex;
        var sprite = leftSlots[slotIndex].sprite;
        if (sprite == null)
            return;
        leftSelectedSlot.sprite = sprite;
        leftSelectedSlot.enabled = true;
    }

    public void SetLeftSlot(int index, Sprite sprite) {
        var slot = leftSlots[index];
        slot.sprite = sprite;
        slot.enabled = true;
        if (leftSelectedIndex == index) {
            SetLeftSelectedSlot(leftSelectedIndex);
        }
    }

    public void SetRightSelectedSlot(int slotIndex) {
        rightSelectedIndex = slotIndex;
        rightSelectedSlot.sprite = rightSlots[slotIndex].sprite;
    }

    public void SetRightSlot(int index, Sprite sprite) {
        rightSlots[index].sprite = sprite;
        if (rightSelectedIndex == index) {
            rightSelectedSlot.sprite = rightSlots[index].sprite;
        }
    }

    public void CanvasEnabled(bool value) {
        GetComponent<Canvas>().enabled = value;
    }
}