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
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canMove;
    public PlayerStateInstance GetStateInstance() => StateInstance;

    public virtual void Enter()
    {
        _rigidbody.linearDamping = HorizontalDamp;
        Debug.Log($"Entered state: {StateInstance}");
    }
    public virtual void Exit() { }
    public virtual void FrameUpdate()
    {
        if (_stateMachine.IsOnGround() && !_canJump)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Falling);
        }
    }
    public virtual void PhysicsUpdate()
    {
        if (_canMove) MoveHorizontally();
    }
    public virtual void AnimationTriggerEvent() { }
    public virtual void HandleInput() { }

    public void Start()
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
}
