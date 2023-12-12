using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct ResourceCost
{
    public EResourceType ResourceType;
    public int Value;
}

[CreateAssetMenu(menuName = "Kings/Building")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private EBuildingType buildingType;
    [SerializeField] private GameObject[] buildingPrefab;
    [FormerlySerializedAs("cost")] [SerializeField] private ResourceCost[] buildingCost;
    [SerializeField] private ResourceCost[] dailyCost;
    [SerializeField] private ResourceCost[] buildingReward;
    [SerializeField] private ResourceCost[] dailyReward;

    public EBuildingType BuildingType => buildingType;
    public GameObject[] BuildingPrefab => buildingPrefab;
    public ResourceCost[] BuildingCost => buildingCost;
    public ResourceCost[] DailyCost => dailyCost;
    public ResourceCost[] BuildingReward => buildingReward;
    public ResourceCost[] DailyReward => dailyReward;
}