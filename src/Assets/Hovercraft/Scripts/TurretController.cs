using UnityEngine;

[RequireComponent(typeof(TurretGun))]
public class TurretController : MonoBehaviour
{
    private Transform _transform;
    private TurretGun _gun;
    private Transform _target;

    [SerializeField]
    private Transform _cannon;
    [SerializeField]
    private Transform _aim;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private float _turningSpeed;
    [SerializeField]
    public float _aimingRange;
    [SerializeField]
    public float _shootingRange;

    private void Start()
    {
        _transform = transform;
        _gun = GetComponent<TurretGun>();

        _target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (IsTargetWithinRange(_aimingRange)) {
            Movement();

            if (IsTargetWithinRange(_shootingRange) && IsAiming()) {
                Shooting();
            }
        }
        else {
            if (_lineRenderer.enabled) {
                _lineRenderer.enabled = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!_transform) {
            return;
        }

        Gizmos.color = IsTargetWithinRange(_aimingRange) ? Color.yellow : Color.white;
        Gizmos.DrawWireSphere(_transform.position, _aimingRange);

        Gizmos.color = IsTargetWithinRange(_shootingRange) ? Color.red : Color.white;
        Gizmos.DrawWireSphere(_transform.position, _shootingRange);
    }

    private bool IsTargetWithinRange(float range)
    {
        return Vector3.Distance(_target.transform.position, _transform.position) <= range;
    }

    private void Movement()
    {
        var direction = _target.position - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);
        var lerpRotation = Quaternion.Lerp(_transform.rotation, lookRotation, _turningSpeed * Time.deltaTime).eulerAngles;

        _transform.rotation = Quaternion.Euler(0f, lerpRotation.y, 0f);
    }

    private bool IsAiming()
    {
        var forward = _aim.TransformDirection(Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(_aim.position, forward, out hit) && hit.transform.CompareTag("Player")) {
            _lineRenderer.SetPosition(0, _aim.parent.position);
            _lineRenderer.SetPosition(1, _target.position);

            if (!_lineRenderer.enabled) {
                _lineRenderer.enabled = true;
            }

            return true;
        }

        return false;
    }

    private void Shooting()
    {
        _gun.Shoot(_cannon.position, _cannon.rotation);
    }
}