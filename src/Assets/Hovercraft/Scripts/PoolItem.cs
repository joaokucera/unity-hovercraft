using System;
using UnityEngine;

[Serializable]
public class PoolItem
{
    public GameObject Prefab;
    public int Size;

    public string Tag {
        get {
            return Prefab.tag;
        }
    }
}