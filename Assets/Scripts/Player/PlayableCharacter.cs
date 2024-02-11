using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterMovement))]
public class PlayableCharacter : MonoBehaviour
{
    [Header("Follow Settings")]
    public bool followMode = true; 
    public float threshold = 3.5f;
}
