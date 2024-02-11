using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager instance;

    [Header("PlayerMovement")]
    [SerializeField] GameObject _moveMarkerPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public Action<PlayableCharacter, MarkerComponent> onMarkerSpawn;
    public void CreateMarkerOnClick(PlayableCharacter focusCharacter, Vector3 pos)
    {
        var createdMarker = Instantiate(_moveMarkerPrefab, pos, Quaternion.identity).GetComponent<MarkerComponent>();

        createdMarker.Initialise(focusCharacter);
        onMarkerSpawn?.Invoke(focusCharacter, createdMarker);
    }

    public void DespawnMarker(MarkerComponent marker)
    {
        Destroy(marker.gameObject);
    }
}
