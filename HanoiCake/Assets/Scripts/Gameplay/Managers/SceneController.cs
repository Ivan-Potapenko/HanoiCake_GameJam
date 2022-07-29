using UnityEngine;
using Events;
using System;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static SceneController instance = null;

    [SerializeField]
    private EventListener _loadNextLevelEventListener;

    [SerializeField]
    private EventListener _restartLevelEventListener;

    [SerializeField]
    private EventListener _loadMenuSceneEventListener;

    [SerializeField]
    private EventListener _startGameEventListener;

    [SerializeField]
    private EventListener _exitGameEventListener;

    public Action onLoadMenu = delegate { };
    public Action onLoadGame = delegate { };


    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    private void OnEnable() {
        _loadNextLevelEventListener.ActionsToDo += LoadNextLevel;
        _restartLevelEventListener.ActionsToDo += RestartLevel;
        _loadMenuSceneEventListener.ActionsToDo += LoadMenuScene;
        _startGameEventListener.ActionsToDo += StartGame;
        _exitGameEventListener.ActionsToDo += Exit;
    }

    private void OnDisable() {
        _loadNextLevelEventListener.ActionsToDo -= LoadNextLevel;
        _restartLevelEventListener.ActionsToDo -= RestartLevel;
        _loadMenuSceneEventListener.ActionsToDo -= LoadMenuScene;
        _startGameEventListener.ActionsToDo -= StartGame;
        _exitGameEventListener.ActionsToDo -= Exit;
    }

    private void LoadNextLevel() {
        var nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextScene >= SceneManager.sceneCountInBuildSettings) {
            LoadScene(0);
        }
        else {
            LoadScene(nextScene);
        }
    }

    private void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadMenuScene() {
        LoadScene(0);
    }

    private void StartGame() {
        LoadScene(1);
    }

    private void Exit() {
        Application.Quit();
    }

    private void LoadScene(int index) {
        if(index == 0) {
            onLoadMenu.Invoke();
        }
        else {
            onLoadGame.Invoke();
        }
        SceneManager.LoadScene(index);
    }

}
