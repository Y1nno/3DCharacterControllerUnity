using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovementStateMachine : MonoBehaviour
{
    private PlayerState _currentState;
    private PlayerStateInstance _currentStateInstance;
    private Dictionary<PlayerStateInstance, PlayerState> _stateDictionary = new Dictionary<PlayerStateInstance, PlayerState>();
    [SerializeField] private PlayerStateInstance StartingStateInstance;
    private PlayerController _controller;

    //Animation
    private Animator _animator;
    private int _horizontalInputAnimHash;
    private int _verticalInputAnimHash;

    public void Start()
    {
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _horizontalInputAnimHash = Animator.StringToHash("Horizontal");
        _verticalInputAnimHash = Animator.StringToHash("Vertical");
        Initialize(StartingStateInstance);
    }

    public void Update()
    {
        _currentState?.FrameUpdate();
        UpdateAnimatorValues();
    }

    public void FixedUpdate()
    {
        _currentState?.PhysicsUpdate();
    }

    private void Initialize(PlayerStateInstance initialStateInstance)
    {
        List<PlayerState> states = new List<PlayerState>(GetComponentsInChildren<PlayerState>());
        foreach (PlayerState state in states)
        {
            PlayerStateInstance instance = state.GetStateInstance();
            if (!_stateDictionary.ContainsKey(instance))
            {
                _stateDictionary.Add(instance, state);
            }
            else
            {
                Debug.LogWarning($"Duplicate state instance {instance} found on {state.name}. This state will not be added to the state machine.");
            }
        }
        if (_stateDictionary.TryGetValue(initialStateInstance, out PlayerState initialState))
        {
            _currentState = initialState;
            _currentState?.Enter();
        }
        else
        {
            Debug.LogWarning($"Attempted to initialize a state that does not exist in the state dictionary: {initialStateInstance}");
        }
    }

    public void ChangeState(PlayerStateInstance newStateInstance)
    {
        if (_stateDictionary.TryGetValue(newStateInstance, out PlayerState newState))
        {
            if (newState == _currentState) return;
            if (newState == null)
            {
                Debug.LogWarning("Attempted to change to a state that does not exist in StatePairs.");
                return;
            }
            _currentState?.Exit();
            _currentState = newState;
            _currentStateInstance = newStateInstance;
            _currentState?.Enter();
        }
    }

    #region Animation
    public void UpdateAnimatorValues()
    {
        float snappedHorizontal = SnapByClosestPoint(_controller.InputDir.x, new List<float> {-1f, -0.55f, 0f, 0.55f, 1f});
        float snappedVertical = SnapByClosestPoint(_controller.InputDir.y, new List<float> {-1f, -0.55f, 0f, 0.55f, 1f});
        _animator.SetFloat(_horizontalInputAnimHash, snappedHorizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat(_verticalInputAnimHash, snappedVertical, 0.1f, Time.deltaTime);
    }

    public static float SnapByClosestPoint(float x, List<float> points)
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
    #endregion
}

public enum PlayerStateInstance
{
    Idle,
    Walking,
    Running,
    Jumping,
    Falling
}
