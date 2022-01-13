using UnityEngine;
using UnityEngine.Events;

public class QuestCompletion : MonoBehaviour {
    [SerializeField] private Quest quest;
    [SerializeField] private string[] objectives;
    private QuestList playerQuestList;

    private void Start() {
        playerQuestList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
    }

    public void CompleteObjective(int objectiveIndex) {
        if (playerQuestList != null) {
            playerQuestList.CompleteQuestObjective(quest, objectives[objectiveIndex]);
        }
    }
}