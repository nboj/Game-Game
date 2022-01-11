using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Saving {
    public class SavingWrapper : MonoBehaviour {
        const string saveFileName = "save";
        private SavingSystem savingSystem; 

        private IEnumerator Start() {
            savingSystem = GetComponent<SavingSystem>();
            yield return savingSystem.LoadLastScene(saveFileName); 
        }

        private void Save() {
            savingSystem.Save(saveFileName);
        }

        private void Load() {
            savingSystem.Load(saveFileName);
        }

        private void Update() {
            if (Keyboard.current.kKey.wasReleasedThisFrame) {
                Save();
            } else if (Keyboard.current.lKey.wasReleasedThisFrame) {
                Load();
            }
        }
    }
}