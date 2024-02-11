using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    NavMeshAgent _agent;
    Vector3 _targetDestination;
    [Header("Animations")]
    [SerializeField] AnimationControllerList _animatorControllerList;
    Animator _animator;

    PlayableCharacter _character;
    AvatarController _avatar;
    InputManager _input;
    List<PlayableCharacter> _currentLineUp = new(); 


    private void Start()
    {
        TryGetComponent(out _agent);
        _animator = GetComponentInChildren<Animator>();
        _avatar = AvatarController.instance;
        

        GameManager.instance.TryGetComponent(out _input);
        

        if(TryGetComponent(out _character))
        {
            _avatar.OnGroundClick += PlayableMoveTo;
            _input.OnChangedCharacter += OnCharacterChange;
        }
    }

    private void OnDestroy()
    {
        if (TryGetComponent(out _character))
        {
            _avatar.OnGroundClick -= PlayableMoveTo;
            _input.OnChangedCharacter -= OnCharacterChange;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        _targetDestination = destination;
        _agent.SetDestination(_targetDestination);
    }

    void PlayableMoveTo(CharacterMovement movement, Vector3 position)
    {
        _targetDestination = position;

        if (_character != null)
        {
            bool focused = _avatar.party.focus == _character;
            _currentLineUp = _avatar.party.Characters;

            //if were focused, go there!
            if (focused)
                _agent.SetDestination(_targetDestination);

            //if were not focused, but in follow mode AND the leader is in follow mode also.
            if (!focused && _character.followMode && movement._character.followMode)
                FollowLeaderAsync();

            //if were not focused AND not in follow mode (do nothing)
        }
    }

    async void FollowLeaderAsync()
    {
        //find the character in the party infront of you.
        int i = _currentLineUp.IndexOf(_character);
        if (i <= 0)
            i = _currentLineUp.Count;
        var next = _currentLineUp[i - 1];

        if(!next.followMode)
            next = _avatar.party.focus;

        //Follow the character in the party infront of you.
        float distanceToNext = (next.transform.position - transform.position).magnitude;
        if(distanceToNext > _character.threshold)
            _agent.SetDestination(next.transform.position);

        //Delay
        await Task.Delay(400);
        if (!Application.isPlaying) return;
        bool focused = _avatar.party.focus == _character;

        //if we've switched to this character mid walk, continue to destination unhindered
        if (focused)
            _agent.SetDestination(_targetDestination);

        float distanceToEnd = (_targetDestination - transform.position).magnitude;
        //If we're still too far away from destination, keep following your leader.
        //if we've switched to this character mid walk, we need to break the cycle.
        if(distanceToEnd > _character.threshold && !focused)
        {
            //Loop.
            FollowLeaderAsync();
        }
        else if(!focused)
        {
            //When close enough to destination, just step to the side.
            int rand1 = (int)Random.Range(-_character.threshold, _character.threshold);
            int rand2 = (int)Random.Range(-_character.threshold, _character.threshold);
            Vector3 pos = new(rand1, 0, rand2);
            pos += transform.position;
            _agent.SetDestination(pos);
        }
    }

    private void OnCharacterChange(UnityEngine.InputSystem.InputAction.CallbackContext context, bool left)
    {
        //When we change the character lets make sure the list is updated to avoid sync errors
        _currentLineUp = _avatar.party.Characters;
    }

    public void ToggleFollowMode()
    {
        _character.followMode = !_character.followMode;

        //If we are turning follow mode back, make the other party members walk up to the focus character
        if(_character.followMode)
            foreach(var character in _currentLineUp)
                if(character != _character)
                {
                    character.TryGetComponent(out CharacterMovement move);
                    move?.PlayableMoveTo(this, transform.position);
                }
  
        Debug.Log("Follow Mode has been toggled to " + _character.followMode);
        Debug.LogWarning("Put Feedback Here");
    }

    private void Update()
    {
        HandleAnimations();
    }

    void HandleAnimations()
    {
        //Movement animations
        int index = 0;  //Idle
        if (_agent.velocity.magnitude >= 2f)
            index = 1;  //Walk

        _animator.runtimeAnimatorController = _animatorControllerList.controllers[index];
    }
}
