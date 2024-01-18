using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : MonoBehaviour
{    
    [SerializeField] int startingBuildings = 9;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField, Range(0,1)] float buildNearChance = 0.25f;
    [SerializeField] float distanceVariation = 10f;
    [SerializeField] private BuildingData[] buildingData;
    private Dictionary<EBuildingType, BuildingData> _buildingDataByType = new Dictionary<EBuildingType, BuildingData>();
    [SerializeField] private LayerMask buildingsMask;
    private float _maxBuiltRadius;    
    //private List<TileData> _tiles = new List<TileData>();
    private Dictionary<EDirection, SectionData> _tilesByDirection = new Dictionary<EDirection, SectionData>();
    //public List<TileData> Tiles => _tiles;
    public BuildingData[] Data => buildingData;

    public Dictionary<EDirection, SectionData> TilesByDirection => _tilesByDirection;

    [SerializeField] private float debugCycle = 0.1f;

    private void Awake()
    {
        foreach (var data in buildingData)
        {
            _buildingDataByType.Add(data.BuildingType, data);
        }

        for (int i = 0; i < (int)EDirection.NUM; i++)
        {
            _tilesByDirection.Add((EDirection)i, new SectionData(minRadius));
        }        
    }

    private void Start() 
    {
        for (int i = 0; i < startingBuildings; i++)
        {
           BuildRequest request = new BuildRequest((EBuildingType)Random.Range(0, (int)EBuildingType.NUM), (EDirection)Mathf.Repeat(i, (int)EDirection.NUM)); 
           Build(request);
        }
    }

    IEnumerator TestBuildRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(debugCycle);
            Build(new BuildRequest((EBuildingType)Random.Range(0, (int)EBuildingType.NUM), (EDirection)Random.Range(0, (int)EDirection.NUM)));
        }
    }

    public BuildingData GetBuildDataByType(EBuildingType buildingType)
    {
        return _buildingDataByType[buildingType];
    }

    public TileData Build(BuildRequest buildingRequest)
    {
        var data = _buildingDataByType[buildingRequest.BuildingType];
        var newTile = _tilesByDirection[buildingRequest.Direction].CreateTile(data, buildNearChance, minRadius, distanceVariation, maxRadius);
        return  newTile;
    }

    public void SpendBuildingResources(EBuildingType building, EDirection direction)
    { 
        _tilesByDirection[direction].SpendBuildingResources(GetBuildDataByType(building));
    }

    public bool HaveResources(EBuildingType bType, EDirection direction){
        return _tilesByDirection[direction].HaveResources(GetBuildDataByType(bType).BuildingCost);
    }
}