using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMovementStateMachine))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerState : MonoBehaviour
{
    protected static PlayerController _playerController;
    protected static PlayerMovementStateMachine _stateMachine;
    protected static Rigidbody _rigidbody;

    [Header("State Data")]
    [SerializeField] protected float HorizontalDamp = 5f;
    [SerializeField] protected float HorizontalMoveForce = 5f;
    [SerializeField] private PlayerStateInstance StateInstance;
    [SerializeField] private bool _canJump;
    public PlayerStateInstance GetStateInstance() => StateInstance;

    public virtual void Enter()
    {
        _rigidbody.linearDamping = HorizontalDamp;
    }
    public virtual void Exit() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }
    public virtual void HandleInput() { }

    public void Awake()
    {
        _stateMachine = GetComponent<PlayerMovementStateMachine>();
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}
