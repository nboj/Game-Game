using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTooltipSpawner : TooltipSpawner
{
    public override bool CanCreateTooltip() {
        return true;
    }

    public override void UpdateTooltip(GameObject tooltip) {
        var questStatus = GetComponent<QuestItemUI>().QuestStatus; 
        tooltip.GetComponent<QuestTooltipUI>().Setup(questStatus);
    } 
}
