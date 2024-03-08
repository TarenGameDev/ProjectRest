using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableEvent : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent _event;

    public void Interact()
    {
        _event.Invoke();
    }
}
