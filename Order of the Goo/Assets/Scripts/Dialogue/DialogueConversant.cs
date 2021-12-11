using System.Linq;
using UnityEngine;

namespace RPG.Dialogue {
    public class DialogueConversant : MonoBehaviour {
        private Dialogue currentDialogue; 
        private DialogueNode currentNode;
        private DialogueNode.Speaker currentSpeaker;

        public DialogueNode.Speaker CurrentSpeaker {
            get => currentSpeaker;
        }

        public Dialogue CurrentDialogue {
            get => currentDialogue;
            set => currentDialogue = value;
        }

        public string GetText() {
            if (currentNode == null) {
                return ""; 
            }
            return currentNode.Text;
        } 

        public string Next(int index) {
            var children = currentDialogue.GetAllChildren(currentNode).ToArray(); 
            currentNode = children[index];
            currentSpeaker = currentNode.CurrentSpeaker;
            return currentNode.Text;
        }

        public bool HasNext() { 
            if (currentNode.Children.Count <= 0) { 
                return false;
            }
            return true;
        } 

        public string[] GetChildren() {
            var children = currentDialogue.GetAllChildren(currentNode).ToArray();
            var childrenToStrings = new string[children.Length];
            for (var i = 0; i < children.Length; i++) {
                childrenToStrings[i] = children[i].Text;
            }
            return childrenToStrings;
        } 

        public void Setup() {
            currentNode = currentDialogue.GetRootNode();
        }
    }
}
