using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] private GameObject pauseMenuUI;

    private void OnEnable()
    {
        InputManager.Instance.PlayerActions.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        InputManager.Instance.PlayerActions.Pause.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
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
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}