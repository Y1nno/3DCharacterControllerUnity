using UnityEngine;

public class IdleState : PlayerState
{
    public override void FrameUpdate()
    {
        if (_playerController.InputDir != Vector2.zero)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Walking);
            return;
        }
        base.FrameUpdate();
    }
}
