using UnityEngine;
using System;
using System.Collections.Generic;

public class AnimatorManager : MonoBehaviour
{
    private PlayerController _controller;
    private Animator _animator;
    private int _horizontalInputAnimHash;
    private int _verticalInputAnimHash;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
        _horizontalInputAnimHash = Animator.StringToHash("Horizontal");
        _verticalInputAnimHash = Animator.StringToHash("Vertical");
    }

    private void Update()
    {
        UpdateAnimatorValues();
    }

    public void PlayTargetAnimation(string animName)
    {
        _animator.CrossFade(animName, 0.2f);
    }

    private void UpdateAnimatorValues()
    {
        float snappedHorizontal = SnapByClosestPoint(_controller.InputDir.x, new List<float> {-1f, -0.55f, 0f, 0.55f, 1f});
        float snappedVertical = SnapByClosestPoint(_controller.InputDir.y, new List<float> {-1f, -0.55f, 0f, 0.55f, 1f});
        _animator.SetFloat(_horizontalInputAnimHash, snappedHorizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat(_verticalInputAnimHash, snappedVertical, 0.1f, Time.deltaTime);
    }

    private static float SnapByClosestPoint(float x, List<float> points)
    {
        float closestPoint = points[0];
        points.Sort();
        float closestDistance = MathF.Abs(x - closestPoint);

        for (int i = 1; i < points.Count; i++)
        {
            float distance = MathF.Abs(x - points[i]);
            if (distance > closestDistance)
                break;

            closestDistance = distance;
            closestPoint = points[i];
        }

        return closestPoint;
    }
}
