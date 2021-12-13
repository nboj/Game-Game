using System.Linq;
using UnityEngine;

namespace RPG.Dialogue {
    public class DialogueConversant : MonoBehaviour {
        private Dialogue currentDialogue; 
        private DialogueNode currentNode;
        private DialogueNode.Speaker currentSpeaker;
        private GameObject currentConverser;

        public GameObject CurrentConverser {
            get => currentConverser;
            set => currentConverser = value;
        }

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

        public Texture2D GetAvatarTexture() {
            return currentNode.AvatarImage;
        }

        public string GetName() {
            return currentNode.Name;
        }

        public string Next(int index) {
            var children = currentDialogue.GetAllChildren(currentNode).ToArray();
            FireOnExitTriggers();
            currentNode = children[index];
            FireOnEnterTriggers();
            currentSpeaker = currentNode.CurrentSpeaker;
            return currentNode.Text;
        }

        public void FireOnEnterTriggers() {
            foreach (var dialogueTrigger in currentConverser.GetComponents<DialogueTrigger>()) {
                var triggerName = currentNode.OnEnterTrigger;
                if (!string.IsNullOrEmpty(triggerName) && dialogueTrigger.Action.Equals(triggerName)) {
                    dialogueTrigger.Trigger();
                }
            }
        }

        public void FireOnExitTriggers() {
            foreach (var dialogueTrigger in currentConverser.GetComponents<DialogueTrigger>()) {
                var triggerName = currentNode.OnExitTrigger;
                if (!string.IsNullOrEmpty(triggerName) && dialogueTrigger.Action.Equals(triggerName)) {
                    dialogueTrigger.Trigger();
                }
            }
        }

        public bool HasNext() { 
            if (currentNode == null || currentNode.Children.Count <= 0) { 
                FireOnExitTriggers();
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
            FireOnEnterTriggers();
        }
    }
}
