using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.AI;
using UnityEngine.TextCore.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System;

public class AvatarController : MonoBehaviour
{
    #region Variables
    public static AvatarController instance;

    [Header("Camera")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _panSpeed = 5f;
    float _currentPanSpeed = 1f;
    [SerializeField][Range(0,100)][Tooltip("The percentage of the screen from the edge before starting to pan. Default = 10")] int _panThreshold = 10;
    Vector3 _camPosOffset = new(1,1,-1);
    [SerializeField][Range(3f, 25f)] float cameraDistance = 10f;
    [SerializeField] Vector3 _camRot;
    public bool _freeCam;
    private int _screenHeight, _screenWidth;
    private int _heightRangeMin, _heightRangeMax, _widthRangeMax, _widthRangeMin;

    public Party party;
    #endregion

    #region Unity
    private void Awake()
    {
        //Set up Singleton
        if(instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        //Get current resolution
        //Tie to event settings (resizing will break bounds)
        var resolution = Screen.currentResolution;
        _screenWidth = resolution.width;
        _screenHeight = resolution.height;
        _widthRangeMin = (_screenWidth / 100) * _panThreshold;
        _widthRangeMax = (_screenWidth / 100) * (100 - _panThreshold);
        _heightRangeMin = (_screenHeight / 100) * _panThreshold;
        _heightRangeMax = (_screenHeight / 100) * (100 - _panThreshold);

        //Set focus character
        if (party.focus == null && party.Characters[0] != null)
            party.ChangeFocusDefault();
            

        //Set Up Input Controls
        GameManager.instance.TryGetComponent(out InputManager input);
        input.OnChangeCamera += ChangeCameraMode;
        input.OnChangedCharacter += Input_OnChangedCharacter;
        input.OnL_Click += Input_OnLeftClick;
        input.OnFollowModeChange += Input_OnFollowModeChange;
    }
    private void OnDestroy()
    {
        GameManager.instance.TryGetComponent(out InputManager input);
        input.OnChangeCamera -= ChangeCameraMode;
        input.OnChangedCharacter -= Input_OnChangedCharacter;
        input.OnL_Click -= Input_OnLeftClick;
    }
    private void Update()
    {
        ControlPanSpeed();
    }

    private void FixedUpdate()
    {
        CheckForPanCameraMode();
        CheckForFocusCameraMode();

        _mainCamera.transform.localPosition = cameraDistance * _camPosOffset;
    }
    #endregion

    #region Input Events
    private void Input_OnFollowModeChange(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;

        party.focus.TryGetComponent(out CharacterMovement move);

        move?.ToggleFollowMode();
    }

    private void Input_OnLeftClick(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;

        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            if (hit.collider.TryGetComponent(out IInteractable interact))
                interact.Interact();
            else
            {
                //NEW
                if (hit.collider.gameObject.layer != 3) return;

                Debug.Log("hit walker");
                var path = FindObjectOfType<Pathfinder>();
                path.GeneratePath(party.focus.transform.position, hit.point);
            }
                //TESTING
                //MoveOnGroundTo(hit.point);
    }

    private void Input_OnChangedCharacter(InputAction.CallbackContext context, bool left)
    {
        if (party.Characters.Count <= 1)
        {
            Debug.Log("Not enough characters.");
            return;
        }

        if (!context.performed) return;

        List<PlayableCharacter> temp = new();
        PlayableCharacter held;
        if (left)
        {
            //first to end.
            held = party.Characters[0];
            for (int i = 1; i < party.Characters.Count; i++)
                temp.Add(party.Characters[i]);
            temp.Add(held);
        }
        else
        {
            //end to first.
            held = party.Characters[party.Characters.Count - 1];
            temp.Add(held);
            for (int i = 0; i < party.Characters.Count - 1; i++)
                temp.Add(party.Characters[i]);
        }

        party.Characters = temp;
        party.ChangeFocusDefault();
    }
    #endregion

    public event Action<CharacterMovement, Vector3> OnGroundClick;
    void MoveOnGroundTo(Vector3 groundPoint)
    {
        if (!party.focus.TryGetComponent(out CharacterMovement move)) return;

        //move.MoveTo(groundPoint);
        OnGroundClick?.Invoke(move, groundPoint);

        FeedbackManager.instance.CreateMarkerOnClick(party.focus, groundPoint);
    }

    #region Camera
    void CheckForPanCameraMode()
    {
        if (!_freeCam) return;

        var mousePos = Input.mousePosition;
        Vector3 camPos = new Vector3();

        if (mousePos.x <= _widthRangeMin) camPos += GameManager.cardinalLeft;
            
        if (mousePos.x >= _widthRangeMax) camPos += GameManager.cardinalRight;
            
        if (mousePos.y <= _heightRangeMin) camPos += GameManager.cardinalDown;

        if (mousePos.y >= _heightRangeMax) camPos += GameManager.cardinalUp;

        _rampPanSpeed = camPos != Vector3.zero;

        var newAvatarPos = transform.position + camPos;
        transform.position = Vector3.Lerp(transform.position, newAvatarPos, Time.deltaTime * _currentPanSpeed);
    }

    private bool _rampPanSpeed = false;
    void ControlPanSpeed()
    {
        if (!_rampPanSpeed)
        {
            _currentPanSpeed = 1f;
            return;
        }

        _currentPanSpeed = Mathf.Lerp(_currentPanSpeed, _panSpeed, Time.deltaTime);
    }

    void CheckForFocusCameraMode()
    {
        if (party.focus == null || _freeCam) return;

        //lerp to position
        Vector3 targetPos = new(party.focus.transform.position.x, 0, party.focus.transform.position.z);
        _rampPanSpeed = transform.position != targetPos;
        if (transform.position != party.focus.transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
            float dist = (targetPos - transform.position).magnitude;
            if (dist <= 0.05f) transform.position = targetPos;
        }
    }

    void ChangeCameraMode(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _freeCam = !_freeCam;
    }
    #endregion
}

[System.Serializable]
public struct Party
{
    public List<PlayableCharacter> Characters;
    public PlayableCharacter focus { get { return _focus; } }
    PlayableCharacter _focus;

    public void ChangeFocus(PlayableCharacter character)
    {
        _focus = character;
    }

    public void ChangeFocusDefault() => ChangeFocus(Characters[0]);

}
