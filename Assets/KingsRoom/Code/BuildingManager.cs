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
    
    private List<TileData> _tiles = new List<TileData>();
    private Dictionary<EDirection, List<TileData>> _tilesByDirection = new Dictionary<EDirection, List<TileData>>();
    public List<TileData> Tiles => _tiles;

    public BuildingData[] Data => buildingData;

    [SerializeField] private float debugCycle = 0.1f;

    private void Awake()
    {
        foreach (var data in buildingData)
        {
            _buildingDataByType.Add(data.BuildingType, data);
        }

        _maxBuiltRadius = minRadius;

        //StartCoroutine(TestBuildRoutine());
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
        var prefab = ListUtils<GameObject>.GetRandomElement(data.BuildingPrefab);
        var boxCollider = prefab.GetComponent<BoxCollider>();
        
        var angle = (int)buildingRequest.Direction * 90;
        angle += Random.Range(-45, 45);
        
        var buildPosition = Vector3.forward.Rotate(angle, Vector3.up).normalized;

        bool buildNear = Random.Range(0f, 1f) > buildNearChance;

        bool alreadyBuilt = false;

        if (buildNear && _tilesByDirection.TryGetValue(buildingRequest.Direction, out var buildingList))
        {
            var shuffleBag = new ListShuffleBag<TileData>(buildingList, false);

            while (shuffleBag.Count > 0)
            {
                var tile = shuffleBag.Pick();
                if (tile.Building.Points.Count <= 0) continue;

                var newPoint = ListUtils<Transform>.GetRandomElement(tile.Building.Points);
                tile.Building.Points.Remove(newPoint);
                buildPosition = newPoint.position;
                alreadyBuilt = true;
                Destroy(newPoint.gameObject);
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

        TileData newTile = new TileData();
        newTile.Position = buildPosition;
        newTile.Object = Instantiate(prefab, buildPosition, Quaternion.identity);
        newTile.BuildingData = data;
        newTile.Building = newTile.Object.GetComponent<Building>();
        newTile.Object.transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);

        _tiles.Add(newTile);
        
        foreach (var tile in _tiles)
        {
            tile.Building.ProcessPoints();
        }
        
        if(!_tilesByDirection.ContainsKey(buildingRequest.Direction)) _tilesByDirection.Add(buildingRequest.Direction, new List<TileData>());
        _tilesByDirection[buildingRequest.Direction].Add(newTile);

        return newTile;
    }
}
