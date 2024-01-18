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
    //List<MarkerComponent> _spawnedMarkers = new();

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public Action<PlayableCharacter, MarkerComponent> onMarkerSpawn;
    public void CreateMarkerOnClick(PlayableCharacter PC, Vector3 pos)
    {
        //Debug.Log("Create marker here " + PC.name + " " + pos);
        var createdMarker = Instantiate(_moveMarkerPrefab, pos, Quaternion.identity).GetComponent<MarkerComponent>();

        createdMarker.Initialise(PC);
        onMarkerSpawn?.Invoke(PC, createdMarker);
        //_spawnedMarkers.Add(createdMarker);
        //foreach(var marker in _spawnedMarkers)
        //{
        //    if (marker.character == PC && marker != createdMarker)
        //        DespawnMarker(marker);
        //}
    }

    public void DespawnMarker(MarkerComponent marker)
    {
        //_spawnedMarkers.Remove(marker);
        Destroy(marker.gameObject);
    }
}
