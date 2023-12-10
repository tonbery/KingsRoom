using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Transform _finalDestination;

    public void SetDestinationPoint(Transform destination)
    {
        _finalDestination = destination;
        _agent.SetDestination(_finalDestination.position);
    }
}
