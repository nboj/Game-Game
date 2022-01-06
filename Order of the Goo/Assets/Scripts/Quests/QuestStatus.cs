using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestStatus {
    [SerializeField] private Quest quest;
    [SerializeField] private List<string> completedObjectives; // If in this list then the objective is complete

    public QuestStatus() { }

    public QuestStatus(Quest quest) { 
        this.quest = quest;
        completedObjectives = new List<string>();
    }

    public Quest Quest {
        get => quest;
    }

    public List<string> CompletedObjectives {
        get => completedObjectives;
    }

    public bool IsObjectiveComplete(string objective) {
        if (completedObjectives.Contains(objective)) {
            return true;
        } else {
            return false;
        }
    }

    public void CompleteObjective(string objective) {
        if (!completedObjectives.Contains(objective)) {
            completedObjectives.Add(objective);
        }
    }
}