using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMovementStateMachine))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerState : MonoBehaviour
{
    protected static PlayerController _playerController;
    protected static PlayerMovementStateMachine _stateMachine;
    protected static Rigidbody _rigidbody;
    protected static AnimatorManager _animator;

    [Header("State Data")]
    [SerializeField] protected float HorizontalDamp = 5f;
    [SerializeField] protected float HorizontalMoveForce = 5f;
    [SerializeField] private PlayerStateInstance StateInstance;
    [SerializeField] private bool _canJump = false;
    [SerializeField] private bool _canMove = false;

    private float _defaultGravityScale = 5f;
    public PlayerStateInstance GetStateInstance() => StateInstance;

    public virtual void Enter()
    {
        _rigidbody.linearDamping = HorizontalDamp;
    }
    public virtual void Exit() { }
    public virtual void FrameUpdate()
    {
        if (!_stateMachine.IsOnGround() && StateInstance != PlayerStateInstance.Jumping)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Falling);
        }
    }
    public virtual void PhysicsUpdate()
    {
        if (_canMove) MoveHorizontally();
        HandleJump();
        ApplyGravity();
    }
    protected virtual void AnimationTriggerEvent() { }
    protected virtual void HandleInput(){}

    protected virtual void HandleJump()
    {
        if (_canJump && _playerController.IsJumping)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Jumping);
        }
    }

    public virtual void Start()
    {
        _stateMachine = GetComponent<PlayerMovementStateMachine>();
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<AnimatorManager>();
    }

    protected void MoveHorizontally()
    {
        var inputDir = _playerController.InputDir;
        if (inputDir == Vector2.zero) return;
        Vector3 moveDir = _playerController.transform.TransformDirection(new Vector3(inputDir.x, 0, inputDir.y));
        _rigidbody.AddForce(moveDir * HorizontalMoveForce, ForceMode.VelocityChange);
    }

    public virtual void ApplyGravity()
    {
        float previousYVelocity = _rigidbody.linearVelocity.y;
        float newYVelocity = _rigidbody.linearVelocity.y - _defaultGravityScale * Time.deltaTime;
        float nextYVelocity = (previousYVelocity + newYVelocity) / 2f;
        _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, nextYVelocity , _rigidbody.linearVelocity.z);
        //_rigidbody.AddForce(Vector3.down * _defaultGravityScale, ForceMode.Acceleration);
    }
}
