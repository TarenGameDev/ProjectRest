using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    NavMeshAgent _agent;
    Vector3 _targetDestination;

    private void Start()
    {
        TryGetComponent(out _agent);
    }

    public void MoveTo(Vector3 destination)
    {
        _targetDestination = destination;
        _agent.SetDestination(_targetDestination);
        
    }
}
