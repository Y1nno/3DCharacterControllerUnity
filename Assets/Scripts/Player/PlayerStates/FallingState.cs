using UnityEngine;

public class FallingState : PlayerState
{
    private float fallSpeed;
    private float maxFallSpeed = 20f;
    private float fallAcceleration = 9.81f;

    public override void Enter()
    {
        fallSpeed = 0f;
        _animator.PlayTargetAnimation("HumanM@Fall01");
    }

    public override void FrameUpdate()
    {
        if (_stateMachine.IsOnGround())
        {
            _stateMachine.ChangeState(PlayerStateInstance.Idle);
            return;
        }
        ApplyGravity();
    }

    public override void Exit() { }

    private void ApplyGravity()
    {
        fallSpeed -= fallAcceleration * Time.deltaTime;
        fallSpeed = Mathf.Max(fallSpeed, -maxFallSpeed);
        _rigidbody.AddForce(Vector3.down * fallSpeed, ForceMode.Acceleration);
    }
}