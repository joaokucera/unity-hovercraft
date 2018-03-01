using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Transform _transform;
    private UIManager _uiManager;

    [SerializeField]
    private int _lives;

    private void Start()
    {
        _transform = transform;

        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.UpdateLives(_lives);
    }

    public void Hit()
    {
        _lives--;

        if (_lives >= 0) {
            _uiManager.UpdateLives(_lives);
        }

        if (_lives == 0) {
            _transform.DetachChildren();
            gameObject.SetActive(false);

            _uiManager.OnEndGame();
        }
    }
}