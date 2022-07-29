using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _exitButton;

    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Button _loadMenuButton;

    [SerializeField]
    private GameObject _menuGroup;

    [SerializeField]
    private GameObject _gameGroup;

    [SerializeField]
    private GameObject _deadGroup;

    [SerializeField]
    private EventListener _deadEventListener;

    [SerializeField]
    private SceneController _sceneController;

    [SerializeField]
    private EventDispatcher _restartLevelEventDispatcher;
    [SerializeField]
    private EventDispatcher _loadMenuSceneEventDispatcher;
    [SerializeField]
    private EventDispatcher _startGameEventDispatcher;
    [SerializeField]
    private EventDispatcher _exitGameEventDispatcher;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    private void OnEnable() {
        _playButton.onClick.AddListener(_startGameEventDispatcher.Dispatch);
        _exitButton.onClick.AddListener(_exitGameEventDispatcher.Dispatch);
        _restartButton.onClick.AddListener(_restartLevelEventDispatcher.Dispatch);
        _loadMenuButton.onClick.AddListener(_loadMenuSceneEventDispatcher.Dispatch);
        _sceneController.onLoadGame += ShowGame;
        _sceneController.onLoadMenu += ShowMenu;
        _deadEventListener.ActionsToDo += ShowDead;
    }

    private void OnDisable() {
        _playButton.onClick.RemoveListener(_startGameEventDispatcher.Dispatch);
        _exitButton.onClick.RemoveListener(_exitGameEventDispatcher.Dispatch);
        _restartButton.onClick.RemoveListener(_restartLevelEventDispatcher.Dispatch);
        _loadMenuButton.onClick.RemoveListener(_loadMenuSceneEventDispatcher.Dispatch);
        _sceneController.onLoadGame -= ShowGame;
        _sceneController.onLoadMenu -= ShowMenu;
        _deadEventListener.ActionsToDo -= ShowDead;
    }

    private void ShowMenu() {
        _menuGroup.SetActive(true);
        _gameGroup.SetActive(false);
        _deadGroup.SetActive(false);
        Time.timeScale = 1;
    }

    private void ShowGame() {
        _menuGroup.SetActive(false);
        _gameGroup.SetActive(true);
        _deadGroup.SetActive(false);
        Time.timeScale = 1;
    }

    private void ShowDead() {
        _deadGroup.SetActive(true);
    }
}
