using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public Vector3 Position;
    public GameObject Object;
    public BuildingData BuildingData;
    public Building Building;
    public Dictionary<EResourceType, int> Resources = new Dictionary<EResourceType, int>();

    public TileData(Vector3 position, BuildingData data)
    {
        BuildingData = data;
        var prefab = ListUtils<GameObject>.GetRandomElement(data.BuildingPrefab);
        Object = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Building = Object.GetComponent<Building>();
        Object.transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
    }
    
    public void AdvanceDay()
    {

    }
}

public class ResourceWalet
{
    public Dictionary<EResourceType, int> ResourceList = new Dictionary<EResourceType, int>();
    public ResourceWalet(){
         for (int i = 0; i < (int)EResourceType.NUM; i++)
        {
            ResourceList.Add((EResourceType)i, 10);
        }
    }
    public void Add(EResourceType rType, int count){
        ResourceList[rType] += count;
    }

    public void Remove(EResourceType rType, int count){
        ResourceList[rType] -= count;
    }

    public bool Has(EResourceType rType, int count){
        return ResourceList[rType] >= count;
    }
}


public class SectionData
{
    List<TileData> _tiles;
    EDirection _direction;
    float _maxBuiltRadius;

    ResourceWalet ResourceWalet;

    public SectionData(float minRadius){
         _maxBuiltRadius = minRadius;
         ResourceWalet = new ResourceWalet();
    }

    public void ProcessResources()
    {
        foreach (var tile in _tiles)
        {
            if (tile.BuildingData.DailyCost.Length == 0 || HaveResources(tile.BuildingData.DailyCost))
            {
                foreach (var cost in tile.BuildingData.DailyCost)
                {
                    ResourceWalet.Remove(cost.ResourceType, cost.Value);
                }                
                
                foreach (var reward in tile.BuildingData.DailyReward)
                {
                    ResourceWalet.Add(reward.ResourceType, reward.Value);
                }
            }
        }
    }

    public void SpendBuildingResources(BuildingData building)
    {        
        foreach (var cost in building.BuildingCost)
        {
            ResourceWalet.Remove(cost.ResourceType, cost.Value);            
        }
    }
    

    public bool HaveResources(ResourceCost[] costs)
    {
        foreach (var cost in costs)
        {
            if(!ResourceWalet.Has(cost.ResourceType, cost.Value)) return false;            
        }

        return true;
    }

    public TileData CreateTile(BuildingData data, float buildNearChance, float minRadius, float distanceVariation, float maxRadius){
        var angle = (int)_direction * 90;
        angle += Random.Range(-45, 45);
        
        var buildPosition = Vector3.forward.Rotate(angle, Vector3.up).normalized;

        bool buildNear = Random.Range(0f, 1f) > buildNearChance;

        bool alreadyBuilt = false;

        if (buildNear)
        {
            var shuffleBag = new ListShuffleBag<TileData>(_tiles, false);

            while (shuffleBag.Count > 0)
            {
                var tile = shuffleBag.Pick();
                if (tile.Building.Points.Count <= 0) continue;

                var newPoint = ListUtils<Transform>.GetRandomElement(tile.Building.Points);
                tile.Building.Points.Remove(newPoint);
                buildPosition = newPoint.position;
                alreadyBuilt = true;
                GameObject.Destroy(newPoint.gameObject);
                break;
            }
        }

        if (!alreadyBuilt)
        {
            var distance = Mathf.Clamp(Random.Range(minRadius, _maxBuiltRadius + Random.Range(0f, distanceVariation)), minRadius, maxRadius);
            if (_maxBuiltRadius < distance) _maxBuiltRadius = distance;
            buildPosition *= distance;
            Debug.DrawRay(buildPosition, Vector3.up * 100, Color.green, 999999);
        }

        TileData newTile = new TileData(buildPosition, data);
        _tiles.Add(newTile);
        
        foreach (var tile in _tiles)
        {
            tile.Building.ProcessPoints();
        }

        _tiles.Add(newTile);

        foreach (var reward in newTile.BuildingData.BuildingReward)
        {
            ResourceWalet.Add(reward.ResourceType, reward.Value);
            
        }

        return newTile;
    }
    
}