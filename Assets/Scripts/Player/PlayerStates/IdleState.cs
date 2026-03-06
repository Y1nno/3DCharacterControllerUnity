using UnityEngine;

public class IdleState : PlayerState
{
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        var inputDir = _playerController.InputDir;
        _rigidbody.AddForce(new Vector3(inputDir.x, 0, inputDir.y) * HorizontalMoveForce, ForceMode.VelocityChange);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (_playerController.InputDir != Vector2.zero)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Walking);
        }
    }
}
