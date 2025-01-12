using System.Collections;
using DialogueSystem;
using GameEvents;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueGameButton;

    [Header("Load Scenes")]
    [SerializeField] private SceneSO _firstGameScene;
    [SerializeField] private SceneSO _sceneToLoad;

    [Header("Events")]
    [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            _continueGameButton.interactable = false;
        }
        
        _newGameButton.Select();
    }
    
    public void OnNewGameClicked()
    {
        if (_firstGameScene)
        {
            DisableMenuButtons();
            DataPersistenceManager.Instance.NewGame(_firstGameScene.sceneName);
            DataPersistenceManager.Instance.LoadGame(); // initialize variables with default values
            
            DialogueManager.Instance.ResetVariables();

            LoadSceneRequest request = new LoadSceneRequest(_firstGameScene, true);
            if (_loadSceneRequestEvent)
                _loadSceneRequestEvent.Raise(request);
        }
        else
        {
            Debug.LogError("There is no scene to load. Set the SceneSO in the inspector.");
        }
    }

    public void OnContinueGameClicked()
    {
        //string sceneNameToLoad = DataPersistenceManager.Instance.GetGameData().lastSaveScene;
        string sceneNameToLoad = DataPersistenceManager.Instance.GetGameData().lastSaveInfo.sceneName;
        if (ValidSceneName(sceneNameToLoad))
        {
            DisableMenuButtons();
            DataPersistenceManager.Instance.LoadGame(); // initialize variables with saved data

            _sceneToLoad.sceneName = sceneNameToLoad;
            LoadSceneRequest request = new LoadSceneRequest(_sceneToLoad, true);
            if (_loadSceneRequestEvent)
                _loadSceneRequestEvent.Raise(request);
        }
        else
        {
            Debug.LogError($"The scene from the save file doesn't exit -> {sceneNameToLoad}");
        }
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        _newGameButton.interactable = false;
        _continueGameButton.interactable = false;
    }

    private bool ValidSceneName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneFileName == sceneName)
                return true;
        }

        return false;
    }
}
