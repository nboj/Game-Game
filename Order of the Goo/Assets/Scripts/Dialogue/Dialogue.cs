using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue {

    [CreateAssetMenu(menuName = "Dialogue", fileName = "New Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();
        private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        public List<DialogueNode> Nodes {
            get => nodes;
            set {
                nodes = value;
            }
        }

        public void OnValidate() {
            nodeLookup.Clear();
            foreach (var node in nodes) {
                nodeLookup[node.name] = node;
            }
        }

        public DialogueNode GetRootNode() {
            return Nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent) {
            if (parent.Children != null) {
                foreach (var child in parent.Children) {
                    if (nodeLookup.ContainsKey(child))
                        yield return nodeLookup[child];
                }
            }
        }
        public void CreateNewNode(DialogueNode parent) {
            var node = CreateInstance<DialogueNode>();
            node.name = System.Guid.NewGuid().ToString();
            node.Text = "New dialogue text...";
            if (parent != null) {
                parent.Children.Add(node.name);
                var newRect = parent.Rect;
                var newPos = new Vector2(parent.Rect.position.x + parent.Rect.size.x, parent.Rect.position.y);
                newRect.position = newPos;
                node.Rect = newRect;
            }
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) {
                Undo.RecordObject(this, "Created new DialogueNode");
            }
#endif
            nodes.Add(node);
            OnValidate();
        }

        public void RemoveNode(DialogueNode node) {
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) {
                Undo.RecordObject(node, "Deleted node");
            }
#endif
            nodes.Remove(node);
            OnValidate();
            foreach (var n in nodes) {
                n.Children.Remove(node.name);
            }
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(node);
#endif
        }

        public void ChildNode(DialogueNode parentNode, DialogueNode childNode) {
            parentNode.Children.Add(childNode.name);
            OnValidate();
        }

        public void OnBeforeSerialize() {
            if (nodes.Count <= 0) {
                CreateNewNode(null);
            }
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) {
                foreach (DialogueNode node in nodes) {
                    if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(node))) {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize() {
        }
    }
}
