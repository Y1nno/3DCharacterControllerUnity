using System;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : PlayerState
{
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private string JumpStartAnimationName = "Armature|Jump_Start";
    [SerializeField] private float JumpImpulseDelay = 0.1f;

    [Header("Jump Variables")]
    [SerializeField] private float maxJumpTime = .5f;
    [SerializeField] private float maxJumpHeight = 4f;
    private float initialJumpVelocity;
    private float gravity;
    public override void Enter()
    {
        _animator.PlayTargetAnimation(JumpStartAnimationName);
        Invoke(nameof(Jump), JumpImpulseDelay);
    }

    public override void Start()
    {
        base.Start();
        SetUpJumpVariables();
    }

    private void SetUpJumpVariables()
    {
        float timeToApex = maxJumpTime / 2f;
        gravity = (2 * maxJumpHeight) / (timeToApex * timeToApex);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    public override void FrameUpdate()
    {
        if (!_playerController.IsJumping)
        {
            _stateMachine.ChangeState(PlayerStateInstance.Falling);
            return;
        }
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
    }

    public override void Exit() { }

}
