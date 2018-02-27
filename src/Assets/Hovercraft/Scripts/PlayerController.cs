using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    private float _horizontal;
    private float _vertical;

    [SerializeField]
    private float _turningSpeed;
    [SerializeField]
    private float _movementSpeed;

    private void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        var rotation = Quaternion.Euler(0, _horizontal * _turningSpeed * Time.fixedDeltaTime, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * rotation);

        _rigidbody.velocity = _transform.TransformDirection(0, 0, _vertical * _movementSpeed);
    }
}