using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.AI;

public class AvatarController : MonoBehaviour
{
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

    [Header("Party")]
    [SerializeField] List<PlayableCharacter> _partyCharacters = new();
    private PlayableCharacter m_focusCharacter;
    public PlayableCharacter focusCharacter { get { return m_focusCharacter; } set { m_focusCharacter = value; } }

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
        if (focusCharacter == null && _partyCharacters[0] != null)
            focusCharacter = _partyCharacters[0];
            

        //Set Up Input Controls
        GameManager.instance.TryGetComponent(out InputManager input);
        input.OnChangeCamera += ChangeCameraMode;
        input.OnChangedCharacter += Input_OnChangedCharacter;
        input.OnL_Click += Input_OnLeftClick;
    }

    private void OnDestroy()
    {
        GameManager.instance.TryGetComponent(out InputManager input);
        input.OnChangeCamera -= ChangeCameraMode;
        input.OnChangedCharacter -= Input_OnChangedCharacter;
        input.OnL_Click -= Input_OnLeftClick;
    }

    private void Input_OnLeftClick(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;

        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            if(hit.collider.TryGetComponent(out IInteractable interact))
                interact.Interact();
            else
                GroundClicked(hit.point);
    }

    void GroundClicked(Vector3 groundPoint)
    {
        focusCharacter.TryGetComponent(out CharacterMovement move);
        move?.MoveTo(groundPoint);

        FeedbackManager.instance.CreateMarkerOnClick(focusCharacter, groundPoint);
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

    void CheckForPanCameraMode()
    {
        if (!_freeCam) return;

        var mousePos = Input.mousePosition;
        Vector3 camPos = new Vector3();

        if (mousePos.x <= _widthRangeMin)
        {
            camPos.x += -1;
            camPos.z += -1;
        }
            
        if (mousePos.x >= _widthRangeMax)
        {
            camPos.x += +1;
            camPos.z += +1;
        }
            
        if (mousePos.y <= _heightRangeMin)
        {
            camPos.z += -1;
            camPos.x += +1;
        }
            
        if (mousePos.y >= _heightRangeMax)
        {
            camPos.z += 1;
            camPos.x += -1;
        }

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
        if (focusCharacter == null || _freeCam) return;

        //lerp to position
        Vector3 targetPos = new(focusCharacter.transform.position.x, 0, focusCharacter.transform.position.z);
        _rampPanSpeed = transform.position != targetPos;
        if (transform.position != focusCharacter.transform.position)
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
    private void Input_OnChangedCharacter(InputAction.CallbackContext context, bool left)
    {
        if (_partyCharacters.Count <= 1)
        {
            Debug.Log("Not enough characters.");
            return;
        }

        if (!context.performed) return;


        /// Currently(bad): picks the character next in the list and assigns it as the focus 
        /// Needed(good): cycle each entry up or down 1 position on the list, then assign [0] as focus 
        Debug.LogWarning("This needs to be changed!");

        WrongVersion();
        void WrongVersion()
        {
            int pos = _partyCharacters.IndexOf(focusCharacter);
            if (left)
            {
                pos -= 1;
                if (pos < 0)
                    pos = _partyCharacters.Count - 1;
            }
            else
            {
                pos += 1;
                if (pos == _partyCharacters.Count)
                    pos = 0;
            }

            focusCharacter = _partyCharacters[pos];
        }
        
        
    }

    
}
