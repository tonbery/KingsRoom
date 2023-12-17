using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private BuildingData[] buildingData;
    private Dictionary<EBuildingType, BuildingData> _buildingDataByType = new Dictionary<EBuildingType, BuildingData>();
    
    private float _maxBuiltRadius;
    
    private List<TileData> _tiles = new List<TileData>();
    private Dictionary<EDirection, List<TileData>> _tilesByDirection = new Dictionary<EDirection, List<TileData>>();
    public List<TileData> Tiles => _tiles;

    private void Awake()
    {
        foreach (var data in buildingData)
        {
            _buildingDataByType.Add(data.BuildingType, data);
        }

        _maxBuiltRadius = minRadius;

        StartCoroutine(TestBuildRoutine());
    }

    IEnumerator TestBuildRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Build(new BuildRequest((EBuildingType)Random.Range(0, (int)EBuildingType.NUM), EDirection.North));
        }
    }

    public BuildingData GetBuildDataByType(EBuildingType buildingType)
    {
        return _buildingDataByType[buildingType];
    }

    public TileData Build(BuildRequest buildingRequest)
    {
        var data = _buildingDataByType[buildingRequest.BuildingType];

        var angle = (int)buildingRequest.Direction * 90;
        angle += Random.Range(-45, 45);
        
        var buildPosition = Vector3.forward.Rotate(angle, Vector3.up).normalized;

        bool buildNear = Random.Range(0f, 1f) > 0.25f;

        if (buildNear && _tilesByDirection.TryGetValue(buildingRequest.Direction, out var tileList))
        {
            var randomTile = ListUtils<TileData>.GetRandomElement(tileList);
            buildPosition = randomTile.Position.RandomPointAround(8, 4);
            Debug.DrawRay(buildPosition, Vector3.up * 100, Color.red, 999999);
        }
        else
        {
            var distance = Mathf.Clamp(Random.Range(minRadius, _maxBuiltRadius + Random.Range(0f, maxRadius*0.2f)), minRadius, maxRadius);
            if (_maxBuiltRadius < distance) _maxBuiltRadius = distance;
            buildPosition *= distance;
            Debug.DrawRay(buildPosition, Vector3.up * 100, Color.green, 999999);
        }

        

        TileData newTile = new TileData();
        newTile.Position = buildPosition;
        newTile.Object = Instantiate(ListUtils<GameObject>.GetRandomElement(data.BuildingPrefab), buildPosition, Quaternion.identity);
        newTile.BuildingData = data; 
        _tiles.Add(newTile);
        if(!_tilesByDirection.ContainsKey(buildingRequest.Direction)) _tilesByDirection.Add(buildingRequest.Direction, new List<TileData>());
        _tilesByDirection[buildingRequest.Direction].Add(newTile);

        return newTile;
    }
}
