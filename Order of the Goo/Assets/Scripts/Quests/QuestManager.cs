using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    [SerializeField] Quest questToManage;

    private QuestList playerQuestList;

    private void Start() {
        playerQuestList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
        if (playerQuestList == null) {
            Debug.Log("Error: cannot access quest list on player object");
        }
    }

    public void GiveQuest() {
        playerQuestList.AddQuest(questToManage); 
    }

    public void RemoveQuest() {
        playerQuestList.RemoveQuest(questToManage);
    }
}
