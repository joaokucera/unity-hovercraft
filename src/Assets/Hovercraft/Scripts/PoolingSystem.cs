using System.Collections.Generic;
using UnityEngine;

public partial class PoolingSystem : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    [SerializeField]
    private PoolItem[] _pools;

    private void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in _pools) {
            var poolQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++) {
                GameObject obj = Instantiate(pool.Prefab, transform);
                obj.gameObject.SetActive(false);

                poolQueue.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Tag, poolQueue);
        }
    }

    public GameObject Dequeue(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject obj = _poolDictionary[tag].Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        obj.gameObject.SetActive(true);

        _poolDictionary[tag].Enqueue(obj);

        return obj;
    }
}