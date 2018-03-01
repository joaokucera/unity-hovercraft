using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TurretBullet : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    [Range(-0.5f, -0.1f)]
    private float _minDeviation;
    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float _maxDeviation;

    [HideInInspector]
    public event AfterHit OnAfterHit;

    private void OnEnable()
    {
        if (!_transform) { 
            _transform = transform; 
        }
        if (!_rigidbody) {
            _rigidbody = GetComponent<Rigidbody>();   
        }

        var direction = Vector3.forward;
        direction.x = Random.Range(_minDeviation, _maxDeviation);

        _rigidbody.velocity = _transform.TransformDirection(direction * _movementSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player");

        if (isPlayer) {
            var player = other.GetComponent<PlayerHealth>();

            if (player) {
                player.Hit();
            }
        }

        if (OnAfterHit != null) {
            OnAfterHit(isPlayer);
        }

        gameObject.SetActive(false);
    }
}