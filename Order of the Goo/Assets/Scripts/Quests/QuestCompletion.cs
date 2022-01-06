using UnityEngine;
using UnityEngine.Events;

public class QuestCompletion : MonoBehaviour {
    [SerializeField] private Quest quest;
    [SerializeField] private string objective;
    [SerializeField] private UnityEvent OnQuestComplete;
    private QuestList playerQuestList;

    private void Start() {
        playerQuestList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
    }

    public void CompleteObjective() {
        bool completedQuest = playerQuestList.CompleteQuestObjective(quest, objective);
        if (completedQuest) {
            OnQuestComplete.Invoke();
        }
    }
}