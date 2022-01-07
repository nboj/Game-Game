using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public delegate void OnUpdateQuestListUI();
public delegate void OnQuestComplete(Quest quest);
public class QuestList : MonoBehaviour {
    [SerializeField] private List<QuestStatus> statuses;
    public static event OnUpdateQuestListUI UpdateQuestListUI;
    private event OnQuestComplete completeQuest;

    public OnQuestComplete OnQuestComplete {
        get => completeQuest;
        set => completeQuest = value;
    }

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
                completeQuest(quest);
                return true;
            }  
        }
        return false;
    }

    public bool QuestCompleted(Quest quest) {
        foreach(QuestStatus status in statuses) {
            if (status.CompletedObjectives.Count == quest.ObjectiveCount) {
                completeQuest(quest);
                return true;
            }
        }
        return false;
    }

    public bool CompleteQuestObjective(Quest quest, string objective) { 
        for (int i = 0; i < statuses.Count; i++) {
            if (statuses[i].Quest == quest) {
                for (var j = 0; j < quest.ObjectiveCount; j++) {
                    if (objective.Equals(quest.Objectives[j].Name)) {
                        quest.AddCurrentAmount(j);
                        if (quest.Objectives[j].RepeatAmount <= quest.Objectives[j].CurrentAmount) {
                            statuses[i].CompleteObjective(objective);
                            QuestCompleted(quest);
                            UpdateQuestListUI(); 
                            return true;
                        } else {
                            return false;
                        } 
                    }
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
