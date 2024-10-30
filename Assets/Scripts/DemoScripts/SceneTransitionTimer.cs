using GameEvents;
using SceneLoaderSystem;
using UnityEngine;

namespace DemoScripts
{
    public class SceneTransitionTimer : MonoBehaviour
    {
        [SerializeField] private SceneSO _sceneToTransition;
        [SerializeField, Min(0)] private float _timerDuration = 5;

        [Space(10)]
        [SerializeField] private LoadSceneRequestEvent _loadSceneRequestEvent;
        
        private float _timer;
        private bool _hasTriggerTransition;

        private void Start()
        {
            _timer = _timerDuration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0 && !_hasTriggerTransition)
            {
                LoadSceneRequest request = new LoadSceneRequest(_sceneToTransition, true);
                if (_loadSceneRequestEvent)
                    _loadSceneRequestEvent.Raise(request);
            }
        }
    }
}