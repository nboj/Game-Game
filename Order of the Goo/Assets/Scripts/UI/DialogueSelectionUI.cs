using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG.Dialogue;

namespace RPG.UI {
    public class DialogueSelectionUI : DialogueUI {
        [SerializeField] private Button[] buttons;
        [SerializeField] private DialogueUI monologueCanvas;
        public override void Start() {
            dialogueConversant = GameObject.FindWithTag("Player").GetComponent<DialogueConversant>();
            gameObject.SetActive(false);
        } 

        public override void Setup() {
            base.Setup(); 
            SetButtonTexts();
        } 

        public override void Next(int index) { 
            if (HasNext()) {
                dialogueText.text = dialogueConversant.Next(index);
                var isSpeaker = CheckSpeaker();
                if (!isSpeaker) {
                    return;
                }
                SetButtonTexts();
            } else {
                gameObject.SetActive(false);
            }
        }

        private void SetButtonTexts() {
            var children = dialogueConversant.GetChildren();
            for (var i = 0; i < buttons.Length; i++) {
                if (i >= children.Length) {
                    buttons[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    buttons[i].gameObject.SetActive(false);
                } else {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = children[i];
                }
            }
        }

        protected override bool CheckSpeaker() { 
            if (dialogueConversant.CurrentSpeaker == DialogueNode.Speaker.AI) {
                monologueCanvas.gameObject.SetActive(true);
                gameObject.SetActive(false);
                monologueCanvas.Setup();
                return false;
            }
            return true;
        }
    }
}
