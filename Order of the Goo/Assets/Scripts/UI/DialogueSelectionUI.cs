using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG.Dialogue;

namespace RPG.UI {
    public class DialogueSelectionUI : DialogueUI {
        [SerializeField] private Button[] buttons;
        private DialogueUI monologue; 
        public override void Start() {
            monologue = GameObject.FindWithTag("DialogueUI").GetComponent<DialogueUI>();
            dialogueConversant = GameObject.FindWithTag("Player").GetComponent<DialogueConversant>();
            canvas = transform.parent.GetComponent<Canvas>();
            canvas.enabled = false;  
            defaultAvatarSprite = avatarImage.sprite;
        } 

        public override void Setup() {
            base.Setup(); 
            SetButtonTexts();
        } 

        public override void Next(int index) { 
            if (HasNext()) { 
                dialogueText.text = dialogueConversant.Next(index);
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
                SetButtonTexts();
            } else {
                canvas.enabled = false;
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
                monologue.Canvas.enabled = true;
                canvas.enabled = false;
                monologue.Setup();
                return false;
            }
            return true;
        }
    }
}
