using UnityEngine;
using UnityEngine.InputSystem;

public class FallingState : PlayerState
{
    private float _fallSpeed;
    [SerializeField] private float _maxFallSpeed = 20f;
    [SerializeField] private float _fallAcceleration = 9.81f;
    [SerializeField] private string _fallLoopAnimationName = "Armature|Jump_Loop";
    [SerializeField] private string _jumpStartAnimationName = "Armature|Jump_Start";

    public override void Enter()
    {
        base.Enter();
        string currentClipName = _animator.GetCurrentActionAnimation();
        if (currentClipName != _fallLoopAnimationName && currentClipName != _jumpStartAnimationName)
        {
            _animator.PlayTargetAnimationNow(_fallLoopAnimationName);
        }
    }

    public override void FrameUpdate()
    {
        if (_stateMachine.IsOnGround())
        {
            _stateMachine.ChangeState(PlayerStateInstance.Idle);
            return;
        }
    }

    public override void Exit() { }

    public override void ApplyGravity()
    {
        _fallSpeed -= _fallAcceleration * Time.deltaTime;
        _fallSpeed = Mathf.Max(_fallSpeed, -_maxFallSpeed);
        //Debug.Log($"Applying gravity. Current fall speed: {_fallSpeed}");
        _rigidbody.AddForce(Vector3.down * -_fallSpeed, ForceMode.Acceleration);
    }
}