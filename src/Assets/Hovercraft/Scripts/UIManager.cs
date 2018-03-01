using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool _isEndGame;

    [SerializeField]
    private GameObject _gameplayPanel;
    [SerializeField]
    private Text _livesText;
    [SerializeField]
    private Text _gameplayTimeText;
    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private PopupPanel _popupPanel;

    private void Start()
    {
        _menuButton.onClick.AddListener(() => OnClick("Menu"));

        _popupPanel.Initialize(OnClick);
    }

    private void Update()
    {
        if (_isEndGame) {
            return;
        }

        UpdateGameplayTime(Time.timeSinceLevelLoad);
    }

    public void OnEndGame()
    {
        _isEndGame = true;

        _gameplayPanel.gameObject.SetActive(false);
        _popupPanel.Show();

        AudioManager.StopBackgroundMusic();
    }

    public void UpdateLives(int lives)
    {
        _livesText.text = string.Format("Player <<color=#204080FF> {0} </color>>", lives);
    }

    private void UpdateGameplayTime(float currentGameplayTime)
    {
        float minutes = (currentGameplayTime / 60f) % 60;
        float seconds = (currentGameplayTime % 60f);

        _gameplayTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}