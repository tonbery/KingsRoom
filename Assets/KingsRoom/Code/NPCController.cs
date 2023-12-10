using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Transform _finalDestination;

    [SerializeField] private Canvas UICanvas;
    private PlayerController _playerController;


    private void Awake()
    {
        _playerController = GameMode.Instance.PlayerController;
        UICanvas.worldCamera = _playerController.MainCamera;
    }

    private void Update()
    {
        var camForward = _playerController.MainCamera.transform.forward;
        camForward.y = 0;
        UICanvas.transform.forward = camForward;
        
    }

    public void SetDestinationPoint(Transform destination)
    {
        _finalDestination = destination;
        _agent.SetDestination(_finalDestination.position);
    }
}
