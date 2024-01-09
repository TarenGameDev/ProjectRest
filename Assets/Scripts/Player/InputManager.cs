using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;



[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public event Action<InputAction.CallbackContext> OnChangeCamera;
    public void ChangeCamera(InputAction.CallbackContext context) => OnChangeCamera?.Invoke(context);

    public event Action<InputAction.CallbackContext, bool> OnChangedCharacter;
    public void ChangeCharacterLeft(InputAction.CallbackContext context) => OnChangedCharacter?.Invoke(context, true);
    public void ChangeCharacterRight(InputAction.CallbackContext context) => OnChangedCharacter?.Invoke(context, false);

    public event Action<InputAction.CallbackContext> OnClick;
    public void Click(InputAction.CallbackContext context) => OnClick?.Invoke(context);

}
