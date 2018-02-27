using UnityEngine;
using UnityEngine.UI;

public class InterestPointSystem : MonoBehaviour
{
    private struct TurretPoint
    {
        public Transform Target;
        public RectTransform PresentationTransform;
    }

    private TurretPoint[] _points;
    private Camera _mainCamera;

    [SerializeField]
    private Transform _pointsParent;
    [SerializeField]
    private RectTransform _pointPrefab;

    void Start()
    {
        _mainCamera = Camera.main;

        LoadTurrets();
    }

    void LateUpdate()
    {
        for (int i = 0; i < _points.Length; i++) {
            Vector3 direction = _points[i].Target.position - _mainCamera.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, direction, out hit) && hit.transform.CompareTag("Enemy")) {
                if (_points[i].PresentationTransform.gameObject.activeSelf) {
                    _points[i].PresentationTransform.gameObject.SetActive(false);
                }
                continue;
            }

            Vector3 screenPoint = _mainCamera.WorldToScreenPoint(_points[i].Target.position);

            screenPoint.x = Mathf.Clamp(screenPoint.x, _points[i].PresentationTransform.sizeDelta.x, Screen.width - _points[i].PresentationTransform.sizeDelta.x);
            screenPoint.y = Mathf.Clamp(screenPoint.y, _points[i].PresentationTransform.sizeDelta.y, Screen.height - _points[i].PresentationTransform.sizeDelta.y);

            _points[i].PresentationTransform.position = screenPoint;

            //check whether is within the threshold.
            bool isWithinThreshold = _points[i].PresentationTransform.localPosition.y > 0 && _points[i].PresentationTransform.localPosition.z > 0;

            _points[i].PresentationTransform.gameObject.SetActive(isWithinThreshold);
        }
    }

    private void LoadTurrets()
    {
        var turrets = FindObjectsOfType<TurretController>();
        _points = new TurretPoint[turrets.Length];

        for (int i = 0; i < _points.Length; i++) {
            _points[i].Target = turrets[i].transform;
            _points[i].PresentationTransform = Instantiate(_pointPrefab, Vector3.zero, Quaternion.identity, _pointsParent);
        }
    }
}