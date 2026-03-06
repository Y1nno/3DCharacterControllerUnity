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

    [Header("Player Movement Settings")]
    [SerializeField] private float _jumpForce = 5f;

    private Rigidbody _rigidbody;

    private float _onGroundDetectionRayLength = 1.2f;
    private bool _isJumping;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input.MovementAxisEvent += HandleMove;
        _input.JumpActionEvent += HandleJump;
        _input.JumpCancelledEvent += HandleJumpCancelled;
        _input.PauseEvent += HandlePause;
        _input.LookAxisEvent += HandleLook;
    }

    private void FixedUpdate()
    {
        Jump();
    }

    #region Input Handling
    private void HandlePause()
    {
        throw new NotImplementedException();
    }

    private void HandleJumpCancelled()
    {
        _isJumping = false;
    }

    private void HandleJump()
    {
        _isJumping = true;
    }

    private void HandleMove(Vector2 dir)
    {
        _inputDir = dir;
    }

    private void HandleLook(Vector2 dir)
    {
        _lookDir = dir;
    }

    #endregion

    #region Movement

    private void Jump()
    {
        if (_isJumping)
        {
            if (IsOnGround())
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
        }
    }
    #endregion

    #region Conditionals

    private bool IsOnGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _onGroundDetectionRayLength))
        {
            return Vector3.Angle(hit.normal, Vector3.up) < 45f;
        }
        return false;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down * _onGroundDetectionRayLength;

        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _onGroundDetectionRayLength))
        {
            Gizmos.color = Vector3.Angle(hit.normal, Vector3.up) < 45f ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawLine(origin, origin + direction);
    }
    #endregion
}
