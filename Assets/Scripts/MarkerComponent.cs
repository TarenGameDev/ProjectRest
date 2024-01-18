using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerComponent : MonoBehaviour
{
    FeedbackManager feedback;
    //private void Start()
    //{
    //    feedback = FeedbackManager.instance;
    //}

    PlayableCharacter _character;
    public PlayableCharacter character { get { return _character; } }
    LineRenderer _lineRenderer;
    public void Initialise(PlayableCharacter PC)
    {
        _character = PC;
        feedback = FeedbackManager.instance;
        feedback/*FeedbackManager.instance*/.onMarkerSpawn += CheckForExistingMarkers;
        TryGetComponent(out _lineRenderer);
        _lineRenderer.SetPosition(1, transform.position);
    }
    void CheckForExistingMarkers(PlayableCharacter character, MarkerComponent marker)
    {
        if (character == _character && marker != this)
        {
            feedback/*FeedbackManager.instance*/.DespawnMarker(this);
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
        feedback/*FeedbackManager.instance*/.onMarkerSpawn -= CheckForExistingMarkers;
    }
}
