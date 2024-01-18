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

public struct BuildRequest
{
    public EBuildingType BuildingType;
    public EDirection Direction;

    public BuildRequest(EBuildingType newBuildingType, EDirection newDirection)
    {
        BuildingType = newBuildingType;
        Direction = newDirection;
    }
}

public class GameMode : MonoBehaviour
{
    static GameMode _gameMode;
    public static GameMode Instance => _gameMode;
    public PlayerController PlayerController => _playerController;

    public BuildingManager BuildingManager => _buildingManager;

    [SerializeField] private Transform[] NPCSpawnPoints;
    [SerializeField] private NPCEndPoint[] NPCEndPoints;
    [SerializeField] private Transform[] NPCExitPoints;
    [SerializeField] private int dayPhases;
    [SerializeField] private Vector3[] phaseSunRotation;
    [SerializeField] private Light sunRef;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private NightEventData[] nightEvents;
    [SerializeField, Range(0f,1f)] private float nightEventChance;


    private List<BuildRequest> _buildingOfTheDay = new List<BuildRequest>();
        
    int _currentDay;
    private int _currentDayPhase = -1;

    //private Dictionary<EResourceType, int> Resources = new Dictionary<EResourceType, int>();

    private List<NPCRequesterController> _activeNPCs;
    [SerializeField] private NPCDismissController _dismissNPC;
    [SerializeField] private NPCSleepController _sleepNPC;
    
    private ListShuffleBag<NightEventData> _nightEventsBag;
    private ListShuffleBag<BuildingData> _NPCBag;
    private ListShuffleBag<Transform> _spawnPointsBag;
    
    private ListShuffleBag<Transform> _exitPointsBag;    

    private PlayerController _playerController;

    private bool _phaseUnlocked = false;

    
    private void Awake()
    {
        _gameMode = this;
        _playerController = FindObjectOfType<PlayerController>();
    }
    
    private void Start()
    {
        _activeNPCs = new List<NPCRequesterController>();
        
        _nightEventsBag = new ListShuffleBag<NightEventData>(new List<NightEventData>(nightEvents));
        _NPCBag = new ListShuffleBag<BuildingData>(_buildingManager.Data);
        _spawnPointsBag = new ListShuffleBag<Transform>(NPCSpawnPoints);
        _exitPointsBag = new ListShuffleBag<Transform>(NPCExitPoints);

       
        
        TriggerNewPhase();
        _sleepNPC.SetOnGameStateVisible(false);
    }

    public void Sleep()
    {
        if (_currentDayPhase >= dayPhases-1 && _phaseUnlocked)
        {
            ProcessResources();
            
            Build();

            TriggerNightEvent();

            _currentDay++;
            
            Debug.Log("SLEEP");
            _sleepNPC.SetOnGameStateVisible(false);
            _currentDayPhase = -1;
            TriggerNewPhase();
        }
    }

    private void TriggerNightEvent()
    {
        if(_nightEventsBag.ElementsCount <= 0) return;

        while(_nightEventsBag.Count > 1)
        {
            var nightEvent = _nightEventsBag.Pick();
            if(_currentDay < nightEvent.StartDay) continue;

            var eventDirection = (EDirection) Random.Range(0,(int)EDirection.NUM);

            Debug.Log("descontar defesa só de um lado da cidade");

            break;            
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
        _sleepNPC.SetOnGameStateVisible(true);
        
        UnlockPhase();
        
        _dismissNPC.SetOnGameStateVisible(false);
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
        var newNPC = Instantiate(npcPick.NpcPrefab, spawnPoint.position, spawnPoint.rotation);
        newNPC.SetNPCPosition(NPCEndPoints[foundIndex]);
        NPCEndPoints[foundIndex].SetOccupant(newNPC);

        _activeNPCs.Add(newNPC);
    }
    
    public void RequestConstruction(EBuildingType building, EDirection direction)
    {        
        if (!_buildingManager.HaveResources(building, direction))
        {
            return;
        }
        
        _buildingManager.SpendBuildingResources(building, direction);        
        
        _buildingOfTheDay.Add(new BuildRequest(building, direction));

        foreach (var NPC in _activeNPCs)
        {
            NPC.RemoveUI();
        }

        TriggerNewPhase();
    }

    void ProcessResources()
    {
        foreach (var direction in _buildingManager.TilesByDirection)
        {
            direction.Value.ProcessResources();            
        }
    }
    
    void Build()
    {
        foreach (var buildingRequest in _buildingOfTheDay)
        {
            _buildingManager.Build(buildingRequest);            
        }
        
        _buildingOfTheDay.Clear();
    }

    public Transform GetExitPoint()
    {
        return _exitPointsBag.Pick();
    }
     

    private void OnGUI()
    {
        // GUILayout.BeginVertical();
        // foreach (var resource in Resources)
        // {
        //     GUILayout.BeginHorizontal();
        //     GUILayout.Box(resource.Key.ToString());
        //     GUILayout.Box(resource.Value.ToString());
        //     if (GUILayout.Button("+"))
        //     {
        //         Resources[resource.Key] += 5;
        //         break;
        //     }
        //     GUILayout.EndHorizontal();
        // }
        // GUILayout.EndVertical();
    }

    void LockPhase()
    {
        _phaseUnlocked = false;
        foreach (var NPC in _activeNPCs)
        {
            NPC.SetOnGameStateVisible(false);
        }
        
        _dismissNPC.SetOnGameStateVisible(false);
    }

    void UnlockPhase()
    {
        _phaseUnlocked = true;
        
        foreach (var NPC in _activeNPCs)
        {
            NPC.SetOnGameStateVisible(true);
        }
        
        _dismissNPC.SetOnGameStateVisible(true);
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
