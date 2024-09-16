using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPaused = false;
    public GameObject pauseMenu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
        isPaused = !isPaused;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
