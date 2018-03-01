using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TargetPoint : MonoBehaviour
{
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool IsVisible;

    public GameObject InterestPoint;
    public GameObject MarkPoint;

    public RectTransform RectTransform { 
        get; private set; 
    }

    private void Start()
    {
        RectTransform = transform as RectTransform;
    }
}