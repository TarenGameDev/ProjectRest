using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerComponent : MonoBehaviour
{
    FeedbackManager feedback;

    PlayableCharacter _character;
    public PlayableCharacter character { get { return _character; } }
    LineRenderer _lineRenderer;
    public void Initialise(PlayableCharacter PC)
    {
        _character = PC;
        feedback = FeedbackManager.instance;
        feedback.onMarkerSpawn += CheckForExistingMarkers;
        TryGetComponent(out _lineRenderer);
        _lineRenderer.SetPosition(1, transform.position);
    }
    void CheckForExistingMarkers(PlayableCharacter focusCharacter, MarkerComponent marker)
    {
        if (focusCharacter == _character && marker != this)
        {
            feedback.DespawnMarker(this);
        }

        //if we are not the focus character, despawn me
        if(focusCharacter != _character)
        {
            feedback.DespawnMarker(this);
        }
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, _character.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayableCharacter cha)) return;
        if (cha != _character) return;

        FeedbackManager.instance.DespawnMarker(this);
    }

    private void OnDestroy()
    {
        feedback.onMarkerSpawn -= CheckForExistingMarkers;
    }
}
