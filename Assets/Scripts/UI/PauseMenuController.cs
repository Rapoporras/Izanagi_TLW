using System;
using GameEvents;
using SceneLoaderSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _firstSelectedButton;

    [Header("Load Scene")]
    [SerializeField] private SceneSO _mainMenuScene;
    [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;
    
    private bool _isPaused;

    private void Start()
    {
        _panel.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.Instance.UIActions.Pause.performed += OnPause;
        
        _firstSelectedButton.Select();
    }

    private void OnDisable()
    {
        InputManager.Instance.UIActions.Pause.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        InputManager.Instance.DisablePlayerActions();
        _panel.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume()
    {
        InputManager.Instance.EnablePlayerActions();
        _panel.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void Exit()
    {
        // Application.Quit();
        Time.timeScale = 1f;
        LoadSceneRequest request = new LoadSceneRequest(_mainMenuScene, true);
        if (_loadSceneRequestEvent)
            _loadSceneRequestEvent.Raise(request);
    }
}