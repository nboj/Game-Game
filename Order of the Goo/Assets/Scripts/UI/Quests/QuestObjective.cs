using UnityEngine;
using TMPro;

public class QuestObjective : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI objectiveProgress;
    [SerializeField] private TextMeshProUGUI objectiveName;

    public TextMeshProUGUI ObjectiveProgress {
        get => objectiveProgress;
    }

    public TextMeshProUGUI ObjectiveName {
        get => objectiveName;
    }
}