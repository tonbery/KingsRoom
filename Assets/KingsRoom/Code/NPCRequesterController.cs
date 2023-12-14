using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NPCRequesterController : NPCBaseController
{
    [SerializeField] private NPCData myData;
    [SerializeField] private NavMeshAgent _agent;

    private EDirection _myChosenDirection;

    private bool _isExiting;

    private NPCEndPoint _endPoint;

    private Vector3 _currentDestination;
    public EDirection MyChosenDirection => _myChosenDirection;
    public NPCData MyData => myData;
    public NPCEndPoint EndPoint => _endPoint;

    protected override void Start()
    {
        base.Start();
        
        _myChosenDirection = (EDirection)Random.Range(0, (int)EDirection.NUM);
       
        if (myData)
        {
            bool canBuild = GameMode.Instance.HaveResources(myData.BuildingType);
            string message = myData.BuildingType.ToString() + " at " + _myChosenDirection.ToString() + " cost 1 " + canBuild.ToString();
            _NPCCanvas.SetText(message, canBuild);
        }
    }

    protected override void OnAcceptedButtonClicked()
    {
        GameMode.Instance.RequestConstruction(myData.BuildingType, _myChosenDirection);
        SetUIVisibilityRequest(false);
    }

    protected override void Update()
    {
        base.Update();

        if (_isExiting && transform.position.IsClose(_currentDestination, 0.5f))
        {
            Destroy(gameObject);
        }
    }

    public void SetExitPoint(Transform destination)
    {
        _isExiting = true;
        SetDestinationPoint(destination);
    }
    public void SetDestinationPoint(Transform destination)
    {
        _currentDestination = destination.position;
        _agent.SetDestination(destination.position);
    }
    
    public void SetNPCPosition(NPCEndPoint newEndPoint)
    {
        _endPoint = newEndPoint;
        _currentDestination = _endPoint.transform.position;
        SetDestinationPoint(_endPoint.transform);
    }

    public void RemoveUI()
    {
        if(_NPCCanvas) Destroy(_NPCCanvas);
    }
}
