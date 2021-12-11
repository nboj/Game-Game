using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI {
    public class DialogueUI : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI dialogueText;
        [SerializeField] private DialogueSelectionUI selectionCanvas;
        protected DialogueConversant dialogueConversant;

        public virtual void Start() {
            dialogueConversant = GameObject.FindWithTag("Player").GetComponent<DialogueConversant>(); 
            gameObject.SetActive(false);
        }  

        public virtual void Setup() {
            dialogueText.text = dialogueConversant.GetText();
        }

        protected string GetText() {
            return dialogueConversant.GetText();
        }

        public virtual void Next() {
            if (HasNext()) {
                dialogueText.text = dialogueConversant.Next(0);
                var isSpeaker = CheckSpeaker();
                if (!isSpeaker) {
                    return;
                }
            } else {
                gameObject.SetActive(false);
            }
        }

        public virtual void Next(int index) {
            if (HasNext()) {
                dialogueText.text = dialogueConversant.Next(index);
            }
        }

        public bool HasNext() {
            return dialogueConversant.HasNext();
        }

        protected virtual bool CheckSpeaker() { 
            if (dialogueConversant.CurrentSpeaker == DialogueNode.Speaker.PLAYER) {
                selectionCanvas.gameObject.SetActive(true);
                gameObject.SetActive(false);
                selectionCanvas.Setup();
                return false;
            } else {
                return true;
            }
        }
    }
}
