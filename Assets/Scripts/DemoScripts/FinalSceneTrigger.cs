using GameEvents;
using InteractionSystem;
using SceneLoaderSystem;
using UnityEngine;

namespace DemoScripts
{
    public class FinalSceneTrigger : MonoBehaviour, IInteractable
    {
        [Header("Scene Transition")]
        [SerializeField] private SceneSO _finalScene;
        [SerializeField] private LevelEntranceSO _entranceToSpawn;
        [SerializeField] private PlayerPathSO _playerPath;
        [Space(5)]
        [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;
        
        [Header("UI")]
        [SerializeField] private GameObject _interactionText;

        
        public void Interact(Interactor interactor)
        {
            LoadSceneRequest request = new LoadSceneRequest(_finalScene, true);
            _playerPath.levelEntrance = _entranceToSpawn;
            if (_loadSceneRequestEvent)
                _loadSceneRequestEvent.Raise(request);
        }

        public void ShowInteractionUI(bool showUI)
        {
            _interactionText.SetActive(showUI);
        }
    }
}