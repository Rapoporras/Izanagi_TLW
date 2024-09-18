using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SceneLoaderSystem
{
    public class SceneInitializer : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SceneSO[] _sceneDependencies;

        [Header("On Scene Ready")]
        [SerializeField] private UnityEvent _onDependenciesLoaded;

        private void Start()
        {
            StartCoroutine(LoadDependencies());
        }

        private IEnumerator LoadDependencies()
        {
            for (int i = 0; i < _sceneDependencies.Length; i++)
            {
                SceneSO sceneToLoad = _sceneDependencies[i];
                if (!SceneManager.GetSceneByName(sceneToLoad.name).isLoaded)
                {
                    var loadOperation = SceneManager.LoadSceneAsync(sceneToLoad.name, LoadSceneMode.Additive);
                    while (!loadOperation.isDone)
                    {
                        yield return null;
                    }
                }
            }

            _onDependenciesLoaded?.Invoke();
        }
    }
}