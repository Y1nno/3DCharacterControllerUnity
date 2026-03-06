using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;

    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private float _lookSpeed = 5f;
    [SerializeField] private float _pivotSpeed = 5f;

    [SerializeField] private float _minPivot = -30f;
    [SerializeField] private float _maxPivot = 60f;

    [SerializeField] private Transform _target;
    [SerializeField] private PlayerController _playerController;

    private float lookAngle;
    private float pivotAngle;

    private Transform cameraPivot;

    public void Start()
    {
        if (_target == null)
        {
            Debug.LogError("CameraManager: Target is not assigned.");
        }

        cameraPivot = transform.GetComponentInParent<Transform>();
    }

    private void LateUpdate()
    {
        FollowTarget();
        RotateCamera();
    }

    public void FollowTarget()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            _target.position,
            ref _velocity,
            _smoothTime
        );
    }

    public void RotateCamera()
    {
        Vector2 lookDir = _playerController.LookDir;

        lookAngle += lookDir.x * _lookSpeed;
        pivotAngle -= lookDir.y * _pivotSpeed;

        pivotAngle = Mathf.Clamp(pivotAngle, _minPivot, _maxPivot);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            _target.rotation,
            _smoothTime
        );

        cameraPivot.rotation = Quaternion.Euler(pivotAngle, lookAngle, 0);
        RotateTarget();
    }

    private void RotateTarget()
    {
        Quaternion targetRotation = Quaternion.Euler(0, lookAngle, 0);
        _target.GetComponent<PlayerController>().SetTargetRotation(targetRotation);
    }

}