using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private Transform _transform;
    private PoolingSystem _poolingSystem;
    private AudioManager _audioManager;
    private TargetPointSystem _targetPointSystem;
    private List<TargetPoint> _visibleTargets;
    private TargetPoint _targetToDestroy;
    private int _targetIndex;
    private bool _isShooting;

    [SerializeField]
    private string _missileTag;
    [SerializeField]
    private Transform _cannon;
    [SerializeField]
    private Transform _aim;

    private void Start()
    {
        _transform = transform;
        _targetIndex = -1;

        _poolingSystem = FindObjectOfType<PoolingSystem>();
        _audioManager = FindObjectOfType<AudioManager>();

        _targetPointSystem = FindObjectOfType<TargetPointSystem>();
        _targetPointSystem.OnTargetMarkBecameInvisible += ResetTargets;
    }

    private void Update()
    {
        Targeting();

        Shooting();
    }

    private void Targeting()
    {
        int previousTargetIndex = _targetIndex;

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q)) {
            if (_targetIndex == -1) {
                _targetIndex = GetNearestTarget();
            }
            else {
                _targetIndex = -1;
            }
        }

        if (_targetIndex != -1) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                _targetIndex--;

                if (_targetIndex < 0) {
                    _targetIndex = _visibleTargets.Count - 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightShift)) {
                _targetIndex++;

                if (_targetIndex == _visibleTargets.Count) {
                    _targetIndex = 0;
                }
            }
        }

        if (previousTargetIndex != -1) {
            _visibleTargets[previousTargetIndex].MarkPoint.SetActive(false);
        }

        if (_targetIndex != -1) {
            _visibleTargets[_targetIndex].MarkPoint.SetActive(true);
        }
    }

    public void ResetTargets()
    {
        _targetIndex = -1;

        _visibleTargets = null;
    }

    private void Shooting()
    {
        if (!_isShooting && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _targetIndex != -1) {
            _isShooting = true;

            GameObject obj = _poolingSystem.Dequeue(_missileTag, _cannon.position, _cannon.rotation);

            var missile = obj.GetComponent<PlayerMissile>();
            if (missile) {
                _audioManager.PlayPlayerShotClip();
                _targetToDestroy = _visibleTargets[_targetIndex];

                missile.Target = _targetToDestroy.Target;
                missile.OnAfterHit += OnAfterShoot;
            }
        }
    }

    private void OnAfterShoot(bool isHit)
    {
        _audioManager.PlayHitClip();
        _isShooting = false;

        if (isHit) {
            _targetPointSystem.RemovePoint(_targetToDestroy);
        }

        ResetTargets();
    }

    private int GetNearestTarget()
    {
        _visibleTargets = _targetPointSystem.GetVisiblePoints();

        int nearestTargetIndex = -1;
        float distance = Mathf.Infinity;

        for (int i = 0; i < _visibleTargets.Count; i++) {
            float difference = (_visibleTargets[i].Target.position - _transform.position).sqrMagnitude;

            if (difference < distance) {
                distance = difference;

                nearestTargetIndex = i;
            }
        }

        return nearestTargetIndex;
    }
}