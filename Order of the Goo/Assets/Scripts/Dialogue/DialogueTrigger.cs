using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue {
    public class DialogueTrigger : MonoBehaviour {
        [SerializeField] private string action;
        [SerializeField] private UnityEvent onTrigger;

        public string Action {
            get => action;
        }

        public UnityEvent OnTrigger {
            get => onTrigger;
        }

        public void Trigger() {
            onTrigger.Invoke();
        }
    }
}