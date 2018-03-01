using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PopupPanel
{
    public delegate void ButtonClick(string sceneName);

    [SerializeField]
    private GameObject _popup;
    [SerializeField]
    private Button _replayButton;
    [SerializeField]
    private Button _menuButton;

    public void Initialize(ButtonClick onButtonClick)
    {
        _popup.SetActive(false);

        _replayButton.onClick.AddListener(() => onButtonClick("Game"));
        _menuButton.onClick.AddListener(() => onButtonClick("Menu"));
    }

    public void Show()
    {
        _popup.SetActive(true);
    }
}