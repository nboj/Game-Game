using UnityEngine;
using RPG.UI;

namespace RPG.Dialogue {
    public class DialogueActivator : MonoBehaviour, IFHandler {
        [SerializeField] Dialogue dialogue;  
        [SerializeField] DialogueUI dialogueUI;
        [SerializeField] DialogueSelectionUI dialogueSelectionUI;
        private Player player;
        private SpriteRenderer spriteRenderer;
        public Dialogue Dialogue {
            get => dialogue;
        }

        private void Start() { 
            spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            spriteRenderer.color = Color.red;
        }

        private void OnTriggerExit2D(Collider2D collision) {
            spriteRenderer.color = Color.white;
        }

        public void Fire() {
            if (!dialogueUI.isActiveAndEnabled && !dialogueSelectionUI.isActiveAndEnabled) { 
                player.Disable();
                player.DialogueConversant.CurrentDialogue = dialogue;
                player.DialogueConversant.Setup();
                if (dialogue.Nodes[0].CurrentSpeaker == DialogueNode.Speaker.PLAYER) {
                    dialogueSelectionUI.gameObject.SetActive(true);
                    dialogueSelectionUI.Setup();
                } else {
                    dialogueUI.gameObject.SetActive(true);
                    dialogueSelectionUI.Setup();
                }
            } 
        }
    }
}
