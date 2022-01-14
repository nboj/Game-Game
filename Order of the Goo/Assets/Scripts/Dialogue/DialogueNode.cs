using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Dialogue {  
    public class DialogueNode : ScriptableObject { 
        public enum Speaker {
            PLAYER,
            AI,
            OTHER
        }
        [SerializeField] private string text;
        [SerializeField] private List<string> children;
        [SerializeField] private Rect rect = new Rect(0, 0, 200, 240);
        [SerializeField] private Speaker currentSpeaker = Speaker.AI;
        [SerializeField] private Texture2D avatarImage;
        [SerializeField] private string avatarName;
        [SerializeField] private string onEnterTrigger;
        [SerializeField] private string onExitTrigger;

        public string OnEnterTrigger {
            get => onEnterTrigger;
        }

        public string OnExitTrigger {
            get => onExitTrigger;
        }

        public Texture2D AvatarImage {
            get => avatarImage;
            set => avatarImage = value;
        }

        public string AvatarName {
            get => avatarName;
            set => avatarName = value;
        }

        public Speaker CurrentSpeaker {
            get => currentSpeaker;
            set => currentSpeaker = value;
        }

        public string Text {
            get => text; 
            set {
                //if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) 
                    //Undo.RecordObject(this, "Changed Text");
                text = value;
            } 
        }

        public List<string> Children {
            get => children; 
            set { 
                //if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
                  //  Undo.RecordObject(this, "Changed Children");
                children = value;
            } 
        }

        public Rect Rect {
            get => rect; 
            set {
                //if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
                  //  Undo.RecordObject(this, "Changed Rect");
                rect = value;
            } 
        }
    }
}