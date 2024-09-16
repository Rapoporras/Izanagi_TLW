using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string _fristSceneName;

        [Header("UI")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueGameButton;

        private void Start()
        {
            if (!DataPersistenceManager.Instance.HasGameData())
            {
                _continueGameButton.interactable = false;
            }
        }
        
        public void OnNewGameClicked()
        {
            if (string.IsNullOrEmpty(_fristSceneName))
            {
                Debug.LogError("There is no scene to load. Set the scene name in the inspector.");
            }
            else
            {
                DisableMenuButtons();
                DataPersistenceManager.Instance.NewGame();
                SceneManager.LoadSceneAsync(_fristSceneName);
            }
        }

        public void OnContinueGameClicked()
        {
            if (string.IsNullOrEmpty(_fristSceneName))
            {
                Debug.LogError("There is no scene to load. Set the scene name in the inspector.");
            }
            else
            {
                DisableMenuButtons();
                DataPersistenceManager.Instance.LoadGame(); // initialize variables
                SceneManager.LoadSceneAsync(_fristSceneName);
            }
        }

        private void DisableMenuButtons()
        {
            _newGameButton.interactable = false;
            _continueGameButton.interactable = false;
        }
    }
}