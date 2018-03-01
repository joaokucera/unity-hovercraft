using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetPointSystem : MonoBehaviour
{
    private Camera _mainCamera;
    private UIManager _uiManager;
    private List<TargetPoint> _points;

    [SerializeField]
    private Transform _pointsParent;
    [SerializeField]
    private TargetPoint _pointPrefab;

    public delegate void TargetMarkBecameInvisible();
    public event TargetMarkBecameInvisible OnTargetMarkBecameInvisible;

    private void Start()
    {
        _mainCamera = Camera.main;
        _uiManager = FindObjectOfType<UIManager>();

        LoadTurrets();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _points.Count; i++) {
            var screenPoint = _mainCamera.WorldToScreenPoint(_points[i].Target.position);

            screenPoint.x = Mathf.Clamp(screenPoint.x, _points[i].RectTransform.sizeDelta.x, Screen.width - _points[i].RectTransform.sizeDelta.x);
            screenPoint.y = Mathf.Clamp(screenPoint.y, _points[i].RectTransform.sizeDelta.y, Screen.height - _points[i].RectTransform.sizeDelta.y);

            _points[i].RectTransform.position = screenPoint;

            var direction = _points[i].Target.position - _mainCamera.transform.position;

            RaycastHit hit;
            _points[i].IsVisible = Physics.Raycast(_mainCamera.transform.position, direction, out hit) && hit.transform.CompareTag("Enemy");

            if (_points[i].IsVisible) {
                if (_points[i].InterestPoint.activeSelf) {
                    _points[i].InterestPoint.SetActive(false);
                }
            }
            else {
                if (_points[i].MarkPoint.activeSelf) {
                    _points[i].MarkPoint.SetActive(false);

                    if (OnTargetMarkBecameInvisible != null) {
                        OnTargetMarkBecameInvisible();
                    }
                }

                //check whether is within the threshold.
                bool isWithinThreshold = _points[i].RectTransform.localPosition.y > 0 && _points[i].RectTransform.localPosition.z > 0;

                _points[i].InterestPoint.SetActive(isWithinThreshold);
            }
        }
    }

    public List<TargetPoint> GetVisiblePoints()
    {
        return _points.Where(w => w.IsVisible).ToList();
    }

    public void RemovePoint(TargetPoint pointToRemove)
    {
        _points.Remove(pointToRemove);

        Destroy(pointToRemove.gameObject);

        if (_points.Count == 0) {
            _uiManager.OnEndGame();
        }
    }

    private void LoadTurrets()
    {
        _points = new List<TargetPoint>();

        var turrets = FindObjectsOfType<TurretController>();

        for (int i = 0; i < turrets.Length; i++) {
            var point = Instantiate(_pointPrefab, Vector3.zero, Quaternion.identity, _pointsParent);
            point.Target = turrets[i].transform;

            _points.Add(point);
        }
    }
}