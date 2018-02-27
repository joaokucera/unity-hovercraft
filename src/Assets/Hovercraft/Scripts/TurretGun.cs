using UnityEngine;

public class TurretGun : MonoBehaviour
{
    private float _nextFire;

    [SerializeField]
    private PoolingSystem _poolingSystem;
    [SerializeField]
    private string _bulletTag;
    [SerializeField]
    private float _fireRate;

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        if (Time.time > _nextFire) {
            _nextFire = Time.time + _fireRate;

            _poolingSystem.Dequeue(_bulletTag, position, rotation);
        }
    }
}