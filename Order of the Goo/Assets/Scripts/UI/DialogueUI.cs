using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI {
    public class DialogueUI : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI dialogueText;
        [SerializeField] private DialogueSelectionUI selectionCanvas;
        [SerializeField] protected Image avatarImage;
        [SerializeField] protected TextMeshProUGUI avatarName;
        protected DialogueConversant dialogueConversant;
        protected Sprite defaultAvatarSprite;
        protected Canvas canvas;
        
        public Canvas Canvas {
            get => canvas;
            set => canvas = value;
        }

        public virtual void Start() {
            dialogueConversant = GameObject.FindWithTag("Player").GetComponent<DialogueConversant>();
            canvas = transform.parent.GetComponent<Canvas>();
            canvas.enabled = false;
            defaultAvatarSprite = avatarImage.sprite;
        } 
        
        public virtual void Setup() {
            dialogueText.text = dialogueConversant.GetText();
            var texture = dialogueConversant.GetAvatarTexture();
            var name = dialogueConversant.GetName();
            if (name != null) {
                avatarName.text = name;
            }
            if (texture != null) {
                avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            } else {
                avatarImage.sprite = defaultAvatarSprite;
            }
        }

        protected string GetText() {
            return dialogueConversant.GetText();
        }

        public virtual void Next() {
            if (HasNext()) { 
                dialogueText.text = dialogueConversant.Next(0);
                var texture = dialogueConversant.GetAvatarTexture();
                if (texture != null) {
                    avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                } else {
                    avatarImage.sprite = defaultAvatarSprite;
                }
                var name = dialogueConversant.GetName();
                if (name != null) {
                    avatarName.text = name;
                }
                var isSpeaker = CheckSpeaker();
                if (!isSpeaker) {
                    return;
                }
            } else {
                canvas.enabled = false;
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
                selectionCanvas.Canvas.enabled = true;
                canvas.enabled = false;
                selectionCanvas.Setup();
                return false;
            } else {
                return true;
            }
        }
    }
}
