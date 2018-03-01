using UnityEngine;

public delegate void AfterHit(bool isHit);

public class PlayerMissile : MonoBehaviour
{
    private Transform _transform;

    [SerializeField]
    private float _turningSpeed;
    [SerializeField]
    private float _movementSpeed;

    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public event AfterHit OnAfterHit;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (!Target) {
            return;
        }

        var targetPosition = Target.position + Vector3.up;

        Vector3 direction = targetPosition - _transform.position + Random.insideUnitSphere;
        direction.Normalize();

        transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(direction), _turningSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = other.CompareTag("Enemy");

        if (isEnemy) {
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }

        if (OnAfterHit != null) {
            OnAfterHit(isEnemy);
        }

        gameObject.SetActive(false);
    }
}