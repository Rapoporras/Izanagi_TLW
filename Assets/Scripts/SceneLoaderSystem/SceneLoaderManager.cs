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

        // called from listener
        public void OnLoadMenuRequest(LoadSceneRequest request)
        {
            if (!IsSceneAlreadyLoaded(request.scene))
            {
                SceneManager.LoadScene(request.scene.sceneName);
            }
        }

        // called from listener
        public void OnLoadLevelRequest(LoadSceneRequest request)
        {
            if (IsSceneAlreadyLoaded(request.scene))
            {
                ActivateLevel(request);
            }
            else
            {
                if (request.loadingScreen)
                {
                    _pendingRequest = request;
                    _loadingScreenUI.ToggleScreen(true);
                }
                else
                {
                    StartCoroutine(ProcessLevelLoading(request));
                }
            }
        }

        // called from listener
        public void OnLoadingScreenToggled(bool isEnabled)
        {
            if (isEnabled && _pendingRequest != null)
            {
                StartCoroutine(ProcessLevelLoading(_pendingRequest));
            }
        }

        private bool IsSceneAlreadyLoaded(SceneSO scene)
        {
            Scene loadedScene = SceneManager.GetSceneByName(scene.sceneName);

            return loadedScene.IsValid() && loadedScene.isLoaded;
        }

        private IEnumerator ProcessLevelLoading(LoadSceneRequest request)
        {
            if (request.scene)
            {
                var currentLoadedLevel = SceneManager.GetActiveScene();
                SceneManager.UnloadSceneAsync(currentLoadedLevel);

                AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(request.scene.sceneName, LoadSceneMode.Additive);

                while (!loadSceneProcess.isDone)
                {
                    yield return null;
                }

                ActivateLevel(request);
            }
        }

        private void ActivateLevel(LoadSceneRequest request)
        {
            var loadedLevel = SceneManager.GetSceneByName(request.scene.sceneName);
            SceneManager.SetActiveScene(loadedLevel);

            if (request.loadingScreen)
            {
                _loadingScreenUI.ToggleScreen(false);
            }

            _pendingRequest = null;
        }
    }
}