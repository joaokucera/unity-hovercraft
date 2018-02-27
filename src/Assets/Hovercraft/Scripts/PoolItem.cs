using System;
using UnityEngine;

public partial class PoolingSystem
{
    [Serializable]
    public class PoolItem
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }
}