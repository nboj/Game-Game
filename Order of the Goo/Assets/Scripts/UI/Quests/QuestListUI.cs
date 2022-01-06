using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour {
    //[SerializeField] Quest[] tempQuests;
    [SerializeField] QuestItemUI questPrefab;
    // Start is called before the first frame update
    private void Start() {
        QuestList.UpdateQuestListUI += UpdateQuests;
        UpdateQuests();
    } 

    public void UpdateQuests() { 
        transform.DetachChildren();
        var playerQuestList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        foreach (QuestStatus status in playerQuestList.Statuses) {
            var questItemInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            questItemInstance.Setup(status);
        }
    }
}
