using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private int _lives;

    private void Start()
    {
        _uiManager.UpdateLife(_lives);
    }

    public void Hit()
    {
        _lives--;

        if (_lives >= 0) {
            _uiManager.UpdateLife(_lives);
        }

        if (_lives == 0) {
            gameObject.SetActive(false);
        }
    }
}