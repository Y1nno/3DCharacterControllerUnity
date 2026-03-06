using UnityEngine;

public class WalkingState : PlayerState
{
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        var inputDir = _playerController.InputDir;
        if (inputDir == Vector2.zero)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Idle);
            return;
        }
        _rigidbody.AddForce(new Vector3(inputDir.x, 0, inputDir.y) * HorizontalMoveForce, ForceMode.VelocityChange);
    }

}
