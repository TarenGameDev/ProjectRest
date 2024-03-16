using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Vector3 cardinalUp = new(-1,0,1);
    public static Vector3 cardinalDown = new(1, 0, -1);
    public static Vector3 cardinalLeft = new(-1, 0, -1);
    public static Vector3 cardinalRight = new(1, 0, 1);
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    [SerializeField] InputActionAsset _inputActionAsset;
    public InputActionAsset input { get { return _inputActionAsset; } }
}
