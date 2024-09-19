using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoaderSystem
{
    public class SceneLoaderManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LoadingScreenUI _loadingScreenUI;

        private LoadSceneRequest _pendingRequest;

        // Function that will be called from a listener
        public void OnLoadMenuRequest(LoadSceneRequest request)
        {
            if (!IsSceneAlreadyLoaded(request.scene))
            {
                // Menus are loaded instantly
                SceneManager.LoadScene(request.scene.sceneName);
            }
        }

        // Function that will be called from a listener
        public void OnLoadLevelRequest(LoadSceneRequest request)
        {
            if (IsSceneAlreadyLoaded(request.scene))
            {
                // Level is already loaded. Activate it
                ActivateLevel(request);
            }
            else // Level is not loaded
            {
                if (request.loadingScreen)
                {
                    // If a loading screen is requested, then show it and wait
                    _pendingRequest = request;
                    _loadingScreenUI.ToggleScreen(true);
                }
                else
                {
                    // If no loading screen request, load it ASAP
                    StartCoroutine(ProcessLevelLoading(request));
                }
            }
        }

        // Function that will be called from a listener
        public void OnLoadingScreenToggled(bool isEnabled)
        {
            if (isEnabled && _pendingRequest != null)
            {
                // When loading screen is shown, we receive the event and can load the new level
                StartCoroutine(ProcessLevelLoading(_pendingRequest));
            }
        }

        private bool IsSceneAlreadyLoaded(SceneSO scene)
        {
            Scene loadedScene = SceneManager.GetSceneByName(scene.name);

            return loadedScene.IsValid() && loadedScene.isLoaded;
        }

        private IEnumerator ProcessLevelLoading(LoadSceneRequest request)
        {
            if (request.scene)
            {
                var currentLoadedLevel = SceneManager.GetActiveScene();
                SceneManager.UnloadSceneAsync(currentLoadedLevel);

                AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(request.scene.name, LoadSceneMode.Additive);

                // Level is being loaded, it could take some seconds (or not). Waiting until is fully loaded
                while (!loadSceneProcess.isDone)
                {
                    yield return null;
                }

                // Once the level is ready, activate it!
                ActivateLevel(request);
            }
        }

        private void ActivateLevel(LoadSceneRequest request)
        {
            // Set active
            var loadedLevel = SceneManager.GetSceneByName(request.scene.name);
            SceneManager.SetActiveScene(loadedLevel);

            // Hide black loading screen
            if (request.loadingScreen)
            {
                _loadingScreenUI.ToggleScreen(false);
            }

            // Clean status
            _pendingRequest = null;
        }
    }
}