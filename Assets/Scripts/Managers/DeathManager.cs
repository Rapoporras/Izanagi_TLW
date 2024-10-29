using GameEvents;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;

[RequireComponent(typeof(VoidListener))]
public class DeathManager : MonoBehaviour
{
    [Header("Load Scene")]
    [SerializeField] private SceneSO _sceneToLoad;
    [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;

    [Space(10)]
    [SerializeField] private PlayerPathSO _playerPath;
    
    public void ResetScene()
    {
        _playerPath.levelEntrance = _playerPath.lastSavePoint;
        
        TemporalDataManager.Instance.temporalData.Clear();
        
        // _sceneToLoad.sceneName = DataPersistenceManager.Instance.gameData.lastSaveScene;
        _sceneToLoad.sceneName = DataPersistenceManager.Instance.gameData.lastSaveInfo.sceneName;
        LoadSceneRequest request = new LoadSceneRequest(_sceneToLoad, true, true);
        
        if (_loadSceneRequestEvent)
            _loadSceneRequestEvent.Raise(request);
    }
}