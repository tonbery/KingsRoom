using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostDisplay : MonoBehaviour
{
    [SerializeField] private IndividualCostDisplay costPrefab;
    private List<IndividualCostDisplay> _costDisplay;
    private List<IndividualCostDisplay> _gainDisplay;

    public void SetCostDisplay(BuildingData data)
    {
        foreach (var cost in data.BuildingCost)
        {
            CreateDisplay().SetData(new ResourceDisplayData(cost.ResourceType, cost.Value, false, false));
        }

        foreach (var cost in data.DailyCost)
        {
            CreateDisplay().SetData(new ResourceDisplayData(cost.ResourceType, cost.Value, false, true));
        }
        
        foreach (var cost in data.BuildingReward)
        {
            CreateDisplay().SetData(new ResourceDisplayData(cost.ResourceType, cost.Value, true, false));
        }
        
        foreach (var cost in data.DailyReward)
        {
            CreateDisplay().SetData(new ResourceDisplayData(cost.ResourceType, cost.Value, true, true));
        }
    }

    IndividualCostDisplay CreateDisplay()
    {
        var newDisplay = Instantiate(costPrefab, transform, false);
        var newDisplayTransform = newDisplay.transform;
        newDisplayTransform.localPosition = Vector3.zero;
        newDisplayTransform.localScale = Vector3.one;
        return newDisplay;
    }
}
