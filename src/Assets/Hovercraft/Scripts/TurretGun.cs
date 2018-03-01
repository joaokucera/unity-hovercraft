using UnityEngine;

public class TurretGun : MonoBehaviour
{
    private PoolingSystem _poolingSystem;
    private AudioManager _audioManager;
    private float _nextFire;

    [SerializeField]
    private string _bulletTag;
    [SerializeField]
    private float _fireRate;

    private void Start()
    {
        _poolingSystem = FindObjectOfType<PoolingSystem>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        if (Time.time > _nextFire) {
            _nextFire = Time.time + _fireRate;

            GameObject obj = _poolingSystem.Dequeue(_bulletTag, position, rotation);

            var bullet = obj.GetComponent<TurretBullet>();
            if (bullet) {
                _audioManager.PlayEnemyShotClip();

                bullet.OnAfterHit += OnAfterHit;
            }
        }
    }

    private void OnAfterHit(bool isHit)
    {
        _audioManager.PlayHitClip();
    }
}