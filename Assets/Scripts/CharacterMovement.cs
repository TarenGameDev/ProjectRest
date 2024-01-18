using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    NavMeshAgent _agent;
    Vector3 _targetDestination;
    [SerializeField] AnimationControllerList _animatorControllerList;
    Animator _animator;

    private void Start()
    {
        TryGetComponent(out _agent);
        _animator = GetComponentInChildren<Animator>();
    }

    public void MoveTo(Vector3 destination)
    {
        _targetDestination = destination;
        _agent.SetDestination(_targetDestination);
        
    }

    private void Update()
    {
        //Movement animations
        //Idle
        int index = 0;
        if(_agent.velocity.magnitude >= 2f)
        {
            //Walk
            index = 1;
        }
 
        _animator.runtimeAnimatorController = _animatorControllerList.controllers[index];
    }
}
