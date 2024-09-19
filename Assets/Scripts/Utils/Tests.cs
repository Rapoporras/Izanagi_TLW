using System;
using GameEvents;
using SceneLoaderSystem;
using UnityEngine;

namespace Utils
{
    public class Tests : MonoBehaviour
    {
        public SceneSO sceneToTransition;
        public LoadSceneRequestEvent loadSceneRequestEvent;
        
        private LoadSceneRequest _loadSceneRequest;

        private void Awake()
        {
            _loadSceneRequest = new LoadSceneRequest(sceneToTransition, false);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (loadSceneRequestEvent)
                    loadSceneRequestEvent.Raise(_loadSceneRequest);
            }
        }
    }
}