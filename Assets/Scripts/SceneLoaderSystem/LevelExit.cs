using GameEvents;
using UnityEngine;

namespace SceneLoaderSystem
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelExit : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _hasTransition;
        
        [Header("Dependencies")]
        [SerializeField] private SceneSO _sceneToTransition;
        [SerializeField] private LevelEntranceSO _entranceToSpawn;
        [SerializeField] private PlayerPathSO _playerPath;

        [Header("Events")]
        [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;

        private LoadSceneRequest _loadSceneRequest;

        private void Awake()
        {
            _loadSceneRequest = new LoadSceneRequest(_sceneToTransition, _hasTransition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                InputManager.Instance.DisablePlayerActions();
                
                _playerPath.levelEntrance = _entranceToSpawn;
                if (_loadSceneRequestEvent)
                    _loadSceneRequestEvent.Raise(_loadSceneRequest);
            }
        }
    }
}