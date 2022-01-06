using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest", fileName = "New Quest")]
public class Quest : ScriptableObject {
    [Serializable]
    public class Objective {
        [SerializeField] private string objectiveName;
        [SerializeField] private int repeatAmount;
        [NonSerialized] private int currentAmount;
        public Objective(string name, int repeatAmount) {
            this.objectiveName = name;
            this.repeatAmount = repeatAmount; 
            currentAmount = 0;
        } 
        public string Name {
            get => objectiveName; 
        }
        public int RepeatAmount {
            get => repeatAmount; 
        }
        public int CurrentAmount {
            get => currentAmount;
            set => currentAmount = value;
        }
    }
     
    [SerializeField] private string title;  
    [SerializeField] private List<Objective> objectives;

    public string Title {
        get => title;
        set => title = value;
    }

    public int ObjectiveCount {
        get => objectives.Count; 
    }

    public List<Objective> Objectives {
        get => objectives;
        set => objectives = value;
    }

    public void AddCurrentAmount(int index) {
        objectives[index].CurrentAmount++;
    }
}
