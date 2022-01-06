using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestItemUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI progress;
    [SerializeField] private QuestStatus questStatus;

    private Quest quest;

    public Quest Quest {
        get => quest; 
    }

    public QuestStatus QuestStatus {
        get => questStatus;
    }

    public void Setup(QuestStatus questStatus) {
        this.quest = questStatus.Quest;
        this.title.text = quest.Title;
        this.questStatus = questStatus;
        this.progress.text = questStatus.CompletedObjectives.Count + "/" + quest.ObjectiveCount;
    }
}
