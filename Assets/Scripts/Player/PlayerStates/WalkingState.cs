using UnityEngine;

public class WalkingState : PlayerState
{

    public override void Enter()
    {
        base.Enter();
        if (_animator.GetCurrentActionAnimation() != "No clip" && _animator.GetCurrentActionAnimation() != "Armature|Jump_Land")
        {
            _animator.PlayTargetAnimation("New State");
        }
    }

    public override void FrameUpdate()
    {
        var inputDir = _playerController.InputDir;
        if (inputDir == Vector2.zero)
        {
            //Debug.Log("Input is zero, switching to Idle State");
            _stateMachine.ChangeState(PlayerStateInstance.Idle);
            return;
        }
        base.FrameUpdate();
    }

}
