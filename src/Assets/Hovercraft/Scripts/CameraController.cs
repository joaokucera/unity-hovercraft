using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _transform;

    [SerializeField]
    private Transform _target;
    [SerializeField] 
    private float _distance;
    [SerializeField] 
    private float _height;
    [SerializeField] 
    private float _damping;
    [SerializeField] 
    private float _rotationDamping;
    [SerializeField] 
    private float _bumperDistanceCheck;
    [SerializeField] 
    private float _bumperCameraHeight;
    [SerializeField] 
    private Vector3 _bumperRayOffset;

    private void Start()
    {
        _transform = transform;

        _transform.SetParent(_target);
    }

    private void FixedUpdate()
    {
        var wantedPosition = _target.TransformPoint(0, _height, -_distance);
        var lookPosition = _target.TransformPoint(Vector3.zero);

        RaycastHit hit;
        var backwards = _target.transform.TransformDirection(-1 * Vector3.forward);

        if (Physics.Raycast(lookPosition, backwards, out hit, _bumperDistanceCheck) && hit.transform != _target) {
            wantedPosition.x = hit.point.x;
            wantedPosition.z = hit.point.z;
            wantedPosition.y = Mathf.Lerp(hit.point.y + _bumperCameraHeight, wantedPosition.y, Time.deltaTime * _damping);
        }

        _transform.position = Vector3.Lerp(_transform.position, wantedPosition, Time.deltaTime * _damping);

        var wantedRotation = Quaternion.LookRotation(lookPosition - _transform.position, _target.up);

        _transform.rotation = Quaternion.Slerp(_transform.rotation, wantedRotation, Time.deltaTime * _rotationDamping);
    }
}