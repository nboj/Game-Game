using UnityEngine;
using RPG.UI;

namespace RPG.Dialogue {
    public class DialogueActivator : MonoBehaviour, IFHandler {
        [SerializeField] Dialogue dialogue;  
        private DialogueUI dialogueUI;
        private DialogueSelectionUI dialogueSelectionUI;
        private Player player;
        private SpriteRenderer spriteRenderer;
        public Dialogue Dialogue {
            get => dialogue;
            set => dialogue = value;
        }

        private void Start() { 
            dialogueUI = GameObject.FindWithTag("DialogueUI").GetComponent<DialogueUI>();
            dialogueSelectionUI = GameObject.FindWithTag("DialogueSelectionUI").GetComponent<DialogueSelectionUI>();
            spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        } 

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject == player.transform.GetChild(0).gameObject) 
                spriteRenderer.color = Color.red;
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.gameObject == player.transform.GetChild(0).gameObject)
                spriteRenderer.color = Color.white;
        }

        public void Fire() { 
            if (!dialogueUI.Canvas.enabled && !dialogueSelectionUI.Canvas.enabled) { 
                player.Disable(); 
                player.DialogueConversant.CurrentDialogue = dialogue;
                player.DialogueConversant.CurrentConverser = gameObject;
                player.DialogueConversant.Setup();
                if (dialogue.Nodes[0].CurrentSpeaker == DialogueNode.Speaker.PLAYER) {
                    dialogueSelectionUI.Canvas.enabled = true;
                    dialogueSelectionUI.Setup();
                } else {
                    dialogueUI.Canvas.enabled = true;
                    dialogueUI.Setup();
                }
            } 
        }
    }
}
