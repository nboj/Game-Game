using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public delegate void OnUpdateQuestListUI(); 
public class QuestList : MonoBehaviour {
    [SerializeField] private List<QuestStatus> statuses;
    public static event OnUpdateQuestListUI UpdateQuestListUI; 

    public List<QuestStatus> Statuses {
        get => statuses;
        set => statuses = value;
    }

    public void AddQuest(Quest quest) {
        if (HasQuest(quest))
            return;
        QuestStatus newStatus = new QuestStatus(quest);
        statuses.Add(newStatus);
        UpdateQuestListUI();
    }

    public bool HasQuest(Quest quest) {
        foreach (QuestStatus status in statuses) { 
            if (status.Quest == quest) {
                return true;
            }  
        }
        return false;
    }

    public bool QuestCompleted(Quest quest) {
        foreach(QuestStatus status in statuses) {
            if (status.CompletedObjectives.Count == quest.ObjectiveCount) {
                return true;
            }
        }
        return false;
    }

    public bool CompleteQuestObjective(Quest quest, string objective) { 
        for (int i = 0; i < statuses.Count; i++) {
            if (statuses[i].Quest == quest) {
                quest.AddCurrentAmount(i);
                if (quest.Objectives[i].RepeatAmount <= quest.Objectives[i].CurrentAmount) {
                    statuses[i].CompleteObjective(objective);
                    UpdateQuestListUI(); 
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }

    public void RemoveQuest(Quest quest) {
        foreach (QuestStatus status in statuses) {
            if (status.Quest == quest) {
                statuses.Remove(status);
                UpdateQuestListUI();
                return;
            }
        }
    }
}
