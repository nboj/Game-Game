using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestTooltipUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title; 
    [SerializeField] Transform objectiveContainer;
    [SerializeField] GameObject completeObjectivePrefab;
    [SerializeField] GameObject incompleteObjectivePrefab; 

    public void Setup(QuestStatus questStatus) {
        var quest = questStatus.Quest;
        title.text = quest.Title;
        foreach (Transform item in objectiveContainer) {
            Destroy(item.gameObject);
        }
        foreach (Quest.Objective objective in quest.Objectives) {
            var currentObjectivePrefab = questStatus.IsObjectiveComplete(objective.Name) ? completeObjectivePrefab : incompleteObjectivePrefab;
            var objectiveInstance = Instantiate(currentObjectivePrefab, objectiveContainer);
            var title = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            title.text = objective.Name;
            var objectivesUI = objectiveInstance.GetComponentsInChildren<QuestObjective>();
            foreach (QuestObjective objectiveUI in objectivesUI) { 
                if (objectiveUI.ObjectiveName.text.Equals(objective.Name)) {
                    objectiveUI.ObjectiveProgress.text = objective.CurrentAmount.ToString() + "/" + objective.RepeatAmount; 
                } 
            }
        }
    }
}
