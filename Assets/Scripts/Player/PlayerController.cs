using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    private Vector2 _inputDir;
    public Vector2 InputDir => _inputDir;

    private Vector2 _lookDir;
    public Vector2 LookDir => _lookDir;

    [SerializeField] private float minStickDeadzone = 0.1f;

    private Quaternion _targetRotation;

    private void Start()
    {
        _input.MovementAxisEvent += HandleMove;
        _input.JumpActionEvent += HandleJump;
        _input.JumpCancelledEvent += HandleJumpCancelled;
        _input.PauseEvent += HandlePause;
        _input.LookAxisEvent += HandleLook;
    }

    private void Update()
    {
        RotateTowardsTarget();
    }

    #region Input Handling
    private void HandlePause()
    {
        throw new NotImplementedException();
    }

    private void HandleJumpCancelled(){}

    private void HandleJump(){}

    private void HandleMove(Vector2 dir)
    {
        if (dir.magnitude < minStickDeadzone)
            dir = Vector2.zero;
        _inputDir = dir;
    }

    private void HandleLook(Vector2 dir)
    {
        if (dir.magnitude < minStickDeadzone)
            dir = Vector2.zero;
        _lookDir = dir;
    }

    #endregion

    public void SetTargetRotation(Quaternion targetRotation)
    {
        _targetRotation = targetRotation;
    }

    public void RotateTowardsTarget()
    {
        gameObject.transform.rotation = Quaternion.Slerp(
            gameObject.transform.rotation,
            _targetRotation,
            Time.deltaTime * 10f);
    }
}
