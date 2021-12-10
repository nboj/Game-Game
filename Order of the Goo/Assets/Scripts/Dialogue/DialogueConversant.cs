using System.Linq;
using UnityEngine;

namespace RPG.Dialogue {
    public class DialogueConversant : MonoBehaviour {
        [SerializeField] private Dialogue currentDialogue;
        [SerializeField] private Canvas dialogueCanvas;
        private DialogueNode currentNode;

        private void Awake() {
            currentNode = currentDialogue.Nodes[0];
        }

        public string GetText() {
            if (currentNode == null) {
                return ""; 
            }
            return currentNode.Text;
        } 

        public string Next() {
            var children = currentDialogue.GetAllChildren(currentNode).ToArray();
            var randomIndex = Random.Range(0, children.Length);
            currentNode = children[randomIndex];
            return currentNode.Text;
        }

        public bool HasNext() {
            return false;
        }
    }
}
