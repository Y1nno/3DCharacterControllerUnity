using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovementStateMachine : MonoBehaviour
{
    private PlayerState _currentState;
    private PlayerStateInstance _currentStateInstance;
    public PlayerStateInstance CurrentStateInstance => _currentStateInstance;

    private Dictionary<PlayerStateInstance, PlayerState> _stateDictionary = new Dictionary<PlayerStateInstance, PlayerState>();
    [SerializeField] private PlayerStateInstance StartingStateInstance;

    public float OnGroundDetectionRayLength = 0.002f;

    public void Start()
    {
        Initialize(StartingStateInstance);
    }

    public void Update()
    {
        _currentState?.FrameUpdate();
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
            _currentStateInstance = initialStateInstance;
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
            Debug.Log($"Changing state from {_currentStateInstance} to {newStateInstance}");
            _currentState?.Exit();
            _currentState = newState;
            _currentStateInstance = newStateInstance;
            _currentState?.Enter();
        }
    }

    public bool IsOnGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, OnGroundDetectionRayLength))
        {
            return Vector3.Angle(hit.normal, Vector3.up) < 45f;
        }
        return false;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down * OnGroundDetectionRayLength;

        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, OnGroundDetectionRayLength))
        {
            Gizmos.color = Vector3.Angle(hit.normal, Vector3.up) < 45f ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawLine(origin, origin + direction);
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
