using System;
using CustomInput;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Add proper handling for DEBUG print statements

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, GameInput.IPlayerMovementActions, GameInput.IUIActions
{
    private GameInput _gameInput;
    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.PlayerMovement.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
        }
        SetActivePlayerMovement();
    }

    private void OnDisable()
    {
        _gameInput.Disable();
    }

    public event Action<Vector2> MovementAxisEvent;
    public event Action<Vector2> LookAxisEvent;

    public event Action JumpActionEvent;
    public event Action JumpCancelledEvent;
    public event Action ResumeEvent;
    public event Action PauseEvent;

    public void SetActivePlayerMovement()
    {
        _gameInput.UI.Disable();
        _gameInput.PlayerMovement.Enable();
    }

    public void SetActiveUI()
    {
        _gameInput.PlayerMovement.Disable();
        _gameInput.UI.Enable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jump Input. Phase: {context.phase}");
        if (context.phase == InputActionPhase.Performed)
        {
            JumpActionEvent?.Invoke();
            return;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log($"Move Input. Phase: {context.phase}, Value: {context.ReadValue<Vector2>()}");
        MovementAxisEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log($"Look Input. Phase: {context.phase}, Value: {context.ReadValue<Vector2>()}");
        LookAxisEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        //Debug.Log($"Pause Input. Phase: {context.phase}");
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            SetActiveUI();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        //Debug.Log($"Resume Input. Phase: {context.phase}");
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
            SetActivePlayerMovement();
        }
    }
}
