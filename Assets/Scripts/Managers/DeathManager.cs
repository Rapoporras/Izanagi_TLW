using GameEvents;
using GlobalVariables;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VoidListener))]
public class DeathManager : MonoBehaviour
{
    [Header("Load Scene")]
    [SerializeField] private SceneSO _sceneToLoad;
    [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;
    
    [Header("Variables")]
    [SerializeField] private IntReference _playerManna;
    
    [Space(10)]
    [SerializeField] private IntReference _playerHealth;
    [SerializeField] private IntReference _playerMaxHealth;
    
    [Space(10)]
    [SerializeField] private IntReference _playerPotions;
    [SerializeField] private IntReference _playerMaxPotions;
    
    public void ResetScene()
    {
        _sceneToLoad.sceneName = DataPersistenceManager.Instance.gameData.lastSaveScene;
        LoadSceneRequest request = new LoadSceneRequest(_sceneToLoad, true);
        
        if (_loadSceneRequestEvent)
            _loadSceneRequestEvent.Raise(request);
        
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        _playerManna.Value = 0;
        _playerHealth.Value = _playerMaxHealth;
        _playerPotions.Value = _playerMaxPotions;
    }
}