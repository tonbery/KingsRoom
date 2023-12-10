using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{

    [SerializeField] private NPCController[] NPCPrefabs;
    
    [SerializeField] private Transform[] NPCSpawnPoints;
    [SerializeField] private Transform[] NPCEndPoints;
    [SerializeField] private Transform[] NPCExitPoints;

    private List<NPCController> _activeNPCs;

    private ListShuffleBag<NPCController> _NPCBag;
    private ListShuffleBag<Transform> _spawnPointsBag;
    private ListShuffleBag<Transform> _endPointsBag;
    private ListShuffleBag<Transform> _exitPointsBag;

    private void Start()
    {
        _activeNPCs = new List<NPCController>();
        
        _NPCBag = new ListShuffleBag<NPCController>(NPCPrefabs);
        _spawnPointsBag = new ListShuffleBag<Transform>(NPCSpawnPoints);
        _endPointsBag = new ListShuffleBag<Transform>(NPCEndPoints);
        _exitPointsBag = new ListShuffleBag<Transform>(NPCExitPoints);
        
        SpawnNPC();
    }

    void SpawnNPC()
    {
        var spawnPoint = _spawnPointsBag.Pick();
        var newNPC = Instantiate(_NPCBag.Pick(), spawnPoint.position, spawnPoint.rotation);
        newNPC.SetDestinationPoint(_endPointsBag.Pick());
        _activeNPCs.Add(newNPC);
        
        if(_activeNPCs.Count < 3) Invoke(nameof(SpawnNPC), 3);
    }
}
