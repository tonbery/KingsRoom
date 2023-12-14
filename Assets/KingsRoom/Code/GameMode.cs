using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public enum EDirection
{
    North, East, West, South, NUM
}

public enum EBuildingType
{
    House, Farm, Mine, Market, Blacksmith, Tower, Wall, NUM
}

public enum EResourceType
{
    Citizen, Gold, Food, Defense, NUM
}

struct NPCPoint
{
    public Transform Point;
    public bool Occupied;

    public NPCPoint(Transform newPoint)
    {
        Point = newPoint;
        Occupied = false;
    }
}

public class GameMode : MonoBehaviour
{
    static GameMode _gameMode;
    public static GameMode Instance => _gameMode;
    public PlayerController PlayerController => _playerController;

    [SerializeField] private float minRadius = 1;
    [SerializeField] private float maxRadius = 1;
    
    [SerializeField] private NPCData[] NPCDatas;
    [SerializeField] private BuildingData[] buildingData;
    [SerializeField] private Transform[] NPCSpawnPoints;
    [SerializeField] private NPCEndPoint[] NPCEndPoints;
    [SerializeField] private Transform[] NPCExitPoints;
    [SerializeField] private int dayPhases;
    [SerializeField] private Vector3[] phaseSunRotation;
    [SerializeField] private Light sunRef; 
    private int _currentDayPhase = -1;

    private Dictionary<EResourceType, int> Resources = new Dictionary<EResourceType, int>();

    private List<TileData> _tiles;

    private List<NPCRequesterController> _activeNPCs;
    [SerializeField] private NPCDismissController _dismissNPC;
    [SerializeField] private NPCSleepController _sleepNPC;
    

    private ListShuffleBag<NPCData> _NPCBag;
    private ListShuffleBag<Transform> _spawnPointsBag;

    //private ListShuffleBag<Transform> _endPointsBag;
    private ListShuffleBag<Transform> _exitPointsBag;

    private Dictionary<EBuildingType, BuildingData> _buildingDataByType = new Dictionary<EBuildingType, BuildingData>();

    private PlayerController _playerController;

    private bool _phaseUnlocked = false;

    
    private void Awake()
    {
        _gameMode = this;
        _playerController = FindObjectOfType<PlayerController>();
        foreach (var data in buildingData)
        {
            _buildingDataByType.Add(data.BuildingType, data);
        }
    }
    
    private void Start()
    {
        _activeNPCs = new List<NPCRequesterController>();
        
        _NPCBag = new ListShuffleBag<NPCData>(NPCDatas);
        _spawnPointsBag = new ListShuffleBag<Transform>(NPCSpawnPoints);
        _exitPointsBag = new ListShuffleBag<Transform>(NPCExitPoints);

        for (int i = 0; i < (int)EResourceType.NUM; i++)
        {
            Resources.Add((EResourceType)i, 0);
        }

        Resources[EResourceType.Gold] = 10;

        TriggerNewPhase();
        _sleepNPC.SetUIVisibilityState(false);
    }

    public void Sleep()
    {
        if (_currentDayPhase >= dayPhases-1 && _phaseUnlocked)
        {
            Debug.Log("SLEEP");
            _sleepNPC.SetUIVisibilityState(false);
            _currentDayPhase = -1;
            TriggerNewPhase();
            
            Debug.LogError("tem q construir, calcular os recursos, etc");
        }
    }

    void TriggerNewPhase()
    {
        if (_currentDayPhase < dayPhases-1)
        {
            StartCoroutine(StartPhaseRoutine());
        }
        else
        {
            
            StartCoroutine(LastPhaseRoutine());
        }
    }

    private int NPCInterval = 1;
    private int sunTransitionTime = 5;

    IEnumerator LastPhaseRoutine()
    {
        LockPhase();
        
        sunRef.transform.DORotate(phaseSunRotation[_currentDayPhase+1], sunTransitionTime);
        EndPhase();
        yield return new WaitForSeconds(3);
        _sleepNPC.SetUIVisibilityState(true);
        
        UnlockPhase();
        
        _dismissNPC.SetUIVisibilityState(false);
    }

    IEnumerator StartPhaseRoutine()
    {
        LockPhase();
        
        _currentDayPhase++;
        
        if (_activeNPCs.Count > 0)
        {
            EndPhase();
            yield return new WaitForSeconds((NPCInterval * 3) + 3);
        }
        
        sunRef.transform.DORotate(phaseSunRotation[_currentDayPhase], sunTransitionTime);

        yield return new WaitForSeconds(sunTransitionTime);

        for (int i = 0; i < 3; i++)
        {
            SpawnNPC();
            yield return new WaitForSeconds(NPCInterval);
        }
        
        UnlockPhase();
    }

    void EndPhase()
    {
        StartCoroutine(EndPhaseRoutine());
    }

    IEnumerator EndPhaseRoutine()
    {
        foreach (var NPC in _activeNPCs)
        {
            yield return new WaitForSeconds(NPCInterval);
            NPC.SetExitPoint(GetExitPoint());
        }
        
        _activeNPCs.Clear();
    }
    
    
    void SpawnNPC()
    {
        int foundIndex = -1;

        for (int i = 0; i < NPCEndPoints.Length; i++)
        {
            if (NPCEndPoints[i].Occupant == null)
            {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex == -1) return;

        var spawnPoint = _spawnPointsBag.Pick();
        var npcPick = _NPCBag.Pick();
        var newNPC = Instantiate(npcPick.Prefab, spawnPoint.position, spawnPoint.rotation);
        newNPC.SetNPCPosition(NPCEndPoints[foundIndex]);
        NPCEndPoints[foundIndex].SetOccupant(newNPC);
        //newNPC.OnExit.AddListener(OnNPCExit);

        _activeNPCs.Add(newNPC);

        /*foreach (var npcEndPoint in NPCEndPoints)
        {
            if (npcEndPoint.Occupant == null)
            {
                Invoke(nameof(SpawnNPC), 1);
                break;
            }
        }*/
    }

    /*private void OnNPCExit(NPCRequesterController npcRequester)
    {
        npcRequester.EndPoint.SetOccupant(null);
        _activeNPCs.Remove(npcRequester);
        Invoke(nameof(SpawnNPC), 1);
    }*/

    public void RequestConstruction(EBuildingType building, EDirection direction)
    {
        print("REQUEST");

        if (!HaveResources(building))
        {
            Debug.Log("Will not build");
            return;
        }

        SpendBuildingResources(building);
        
        var data = _buildingDataByType[building];

        
        var angle = (int)direction * 90;
        angle += Random.Range(-45, 45);
        
        var vDir = Vector3.forward.Rotate(angle, Vector3.up).normalized;

        vDir *= Random.Range(minRadius, maxRadius);
        
        TileData newTile = new TileData();
        newTile.Position = vDir;
        newTile.Object = Instantiate(ListUtils<GameObject>.GetRandomElement(data.BuildingPrefab), vDir, Quaternion.identity);

        foreach (var NPC in _activeNPCs)
        {
            NPC.RemoveUI();
        }

        TriggerNewPhase();
        
        Debug.LogError("aqui iniciar o lance de chamar proxima etapa do dia");
    }

    public Transform GetExitPoint()
    {
        return _exitPointsBag.Pick();
    }
    
    public bool HaveResources(EBuildingType buildingType)
    {
        var building = _buildingDataByType[buildingType];
        return HaveResources(building);
    }
    
    public void SpendBuildingResources(EBuildingType buildingType)
    {
        var building = _buildingDataByType[buildingType];
        foreach (var cost in building.BuildingCost)
        {
            Resources[cost.ResourceType] -= cost.Value;
        }
    }
    
    public bool HaveResources(BuildingData building)
    {
        foreach (var cost in building.BuildingCost)
        {
            if (Resources[cost.ResourceType] < cost.Value) return false;
        }

        return true;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        foreach (var resource in Resources)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(resource.Key.ToString());
            GUILayout.Box(resource.Value.ToString());
            if (GUILayout.Button("+"))
            {
                Resources[resource.Key] += 5;
                break;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void LockPhase()
    {
        _phaseUnlocked = false;
        foreach (var NPC in _activeNPCs)
        {
            NPC.SetUIVisibilityState(false);
        }
        
        _dismissNPC.SetUIVisibilityState(false);
    }

    void UnlockPhase()
    {
        _phaseUnlocked = true;
        
        foreach (var NPC in _activeNPCs)
        {
            NPC.SetUIVisibilityState(true);
        }
        
        _dismissNPC.SetUIVisibilityState(true);
    }

    public void Dismiss()
    {
        TriggerNewPhase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Time.timeScale *= 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Time.timeScale *= 2f;
        }
        if (Input.GetKeyDown(KeyCode.Home))
        {
            Time.timeScale = 1;
        }
    }
}
