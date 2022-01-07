using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class QuestManager : MonoBehaviour {
    [SerializeField] private Quest questToManage;
    [SerializeField] private UnityEvent onQuestComplete;

    private QuestList playerQuestList;

    public UnityEvent OnQuestComplete {
        get => onQuestComplete;
    }

    private void Start() {
        playerQuestList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
        if (playerQuestList == null) {
            Debug.Log("Error: cannot access quest list on player object");
        }
        playerQuestList.OnQuestComplete += (Quest quest) => {
            if (questToManage == quest) {
                onQuestComplete.Invoke();
            }
        };
    }

    public void GiveQuest() {
        playerQuestList.AddQuest(questToManage); 
    }

    public void CompleteQuest() {
        playerQuestList.RemoveQuest(questToManage);
    }
}
