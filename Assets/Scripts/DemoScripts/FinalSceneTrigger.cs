using GameEvents;
using InteractionSystem;
using SceneLoaderSystem;
using UnityEngine;

namespace DemoScripts
{
    public class FinalSceneTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneSO _finalScene;
        [SerializeField] private GameObject _interactionText;

        [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;
        
        public void Interact(Interactor interactor)
        {
            LoadSceneRequest request = new LoadSceneRequest(_finalScene, true);
            if (_loadSceneRequestEvent)
                _loadSceneRequestEvent.Raise(request);
        }

        public void ShowInteractionUI(bool showUI)
        {
            _interactionText.SetActive(showUI);
        }
    }
}