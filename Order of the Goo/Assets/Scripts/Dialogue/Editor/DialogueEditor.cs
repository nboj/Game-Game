using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace RPG.Dialogue.Editor {
    public class DialogueEditor : EditorWindow {
        private Dialogue selectedDialogue;
        [NonSerialized]
        private GUIStyle nodeStyle;
        [NonSerialized]
        private GUIStyle otherNodeStyle;
        [NonSerialized]
        private DialogueNode draggingNode;
        [NonSerialized]
        private Vector2 dragOffset;
        [NonSerialized]
        private DialogueNode creatingNode = null;
        [NonSerialized]
        private DialogueNode deletingNode = null;
        [NonSerialized]
        private DialogueNode linkingParent = null;
        [NonSerialized]
        private DialogueNode linkingChild = null;
        private Vector2 scrollPos;
        [NonSerialized]
        private bool draggingCanvas = false;
        [NonSerialized]
        private Vector2 draggingCanvasOffset;
        private const float canvasSize = 4000;
        private const float backgroundSize = 50;

        private void OnEnable() {
            Selection.selectionChanged += OnSelectionChanged;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(10, 10, 10, 10);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            nodeStyle.normal.textColor = Color.white;

            otherNodeStyle = new GUIStyle();
            otherNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            otherNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            otherNodeStyle.border = new RectOffset(12, 12, 12, 12);
            otherNodeStyle.normal.textColor = Color.white;
        }

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow() {
            GetWindow<DialogueEditor>(false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line) {
            var instance = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (instance != null) { 
                ShowEditorWindow();
                Selection.selectionChanged();
                return true;
            } 
            return false;
        }

        private void OnSelectionChanged() {
            var instance = Selection.activeObject as Dialogue; 
            if (instance != null) { 
                selectedDialogue = instance;
                Repaint();
            }
        } 

        private void OnGUI() {
            if (selectedDialogue == null) {
                Debug.Log("There is no dialogue selected");
            } else {
                if (Event.current.type == EventType.MouseDrag && draggingNode != null) {
                    OnDrag();
                } else if (Event.current.type == EventType.MouseDrag && draggingCanvas) {
                    scrollPos = draggingCanvasOffset - Event.current.mousePosition;
                    GUI.changed = true;
                } else if (Event.current.type == EventType.MouseDown) {
                    OnMouseDown();
                } else if (Event.current.type == EventType.MouseUp && draggingCanvas) {
                    draggingCanvas = false;
                }

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos); 
                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                var backgroundTexture = Resources.Load("Background") as Texture2D;
                var textCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textCoords);

                foreach (var node in selectedDialogue.Nodes) {
                    DrawNode(node); 
                }
                foreach (var node in selectedDialogue.Nodes) { 
                    DrawConnections(node);
                }
                if (creatingNode != null) {
                    Undo.RecordObject(this, "Created Node");
                    selectedDialogue.CreateNewNode(creatingNode);
                    creatingNode = null;
                    EditorUtility.SetDirty(this);
                } 
                if (deletingNode != null) {
                    Undo.RecordObject(this, "Removed Node");
                    selectedDialogue.RemoveNode(deletingNode);
                    GUI.changed = true;
                    deletingNode = null;
                    EditorUtility.SetDirty(this);
                }
                if (linkingChild != null && linkingParent != null) {
                    Undo.RecordObject(this, "Linked Child");
                    selectedDialogue.ChildNode(linkingParent, linkingChild);
                    linkingChild = null;
                    linkingParent = null;
                    EditorUtility.SetDirty(this);
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawConnections(DialogueNode node) {
            foreach (var childNode in selectedDialogue.GetAllChildren(node)) {
                var startPos = new Vector2(node.Rect.xMax, node.Rect.center.y);
                var endPos = new Vector2(childNode.Rect.xMin, childNode.Rect.center.y);
                var offset = new Vector2(endPos.x - startPos.x, 0);
                offset *= 0.8f;
                Handles.DrawBezier(startPos, endPos, startPos + offset, endPos - offset, Color.red, null, 4f);
            }
        }

        private void OnMouseDown() { 
            var mousePos = Event.current.mousePosition; 
            DialogueNode foundNode = null;
            foreach (var node in selectedDialogue.Nodes) {
                if (node.Rect.Contains(mousePos + scrollPos)) {
                    foundNode = node;
                }
            } 
            draggingNode = foundNode;
            if (draggingNode != null) {
                Selection.activeObject = draggingNode;
                dragOffset = draggingNode.Rect.position - mousePos;
            } else {
                Selection.activeObject = selectedDialogue;
                draggingCanvas = true;
                draggingCanvasOffset = mousePos + scrollPos;
            }
        }

        private void OnDrag() { 
            var mousePos = Event.current.mousePosition;  
            Undo.RecordObject(selectedDialogue, "Moved Object");
            draggingNode.Rect = new Rect(mousePos + dragOffset, draggingNode.Rect.size);
            GUI.changed = true;
        } 

        private void DrawNode(DialogueNode node) {
            var style = nodeStyle;
            if (node.CurrentSpeaker == DialogueNode.Speaker.PLAYER) {
                style = otherNodeStyle;
            } 
            GUILayout.BeginArea(node.Rect, style);
            EditorGUI.BeginChangeCheck(); 
            var text = EditorGUILayout.TextField(node.Text);
            if (EditorGUI.EndChangeCheck()) { 
                node.Text = text; 
            }
            if (GUILayout.Button("New Node")) {
                creatingNode = node;
            } 
            if (GUILayout.Button("Delete Node")) {
                deletingNode = node;
            }
            if (linkingParent == null) {
                if (GUILayout.Button("Link")) {
                    linkingParent = node;
                }
            } else if (linkingParent == node) {
                if (GUILayout.Button("Cancel")) {
                    linkingParent = null;
                    linkingChild = null;
                }
            } else if (linkingParent.Children.Contains(node.name)) {
                if (GUILayout.Button("Unlink")) {
                    linkingParent.Children.Remove(node.name);
                }
            } else {
                if (GUILayout.Button("Child")) {
                    linkingChild = node;
                }
            }

            node.AvatarName = EditorGUILayout.TextField(node.AvatarName);

            node.AvatarImage = (Texture2D)EditorGUILayout.ObjectField(node.AvatarImage, typeof(Texture2D), true);

            if (node.AvatarImage != null) {
                var avatarRect = new Rect(50, 130, 100, 100);
                EditorGUI.DrawPreviewTexture(avatarRect, node.AvatarImage);
            }
            GUILayout.EndArea();
        }
    }
}
