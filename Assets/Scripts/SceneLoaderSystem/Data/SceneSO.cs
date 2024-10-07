using UnityEngine;

namespace SceneLoaderSystem
{
    [CreateAssetMenu(fileName = "NewScene", menuName = "Scene Loader/Scene")]
    public class SceneSO : ScriptableObject
    {
        [Header("Scene Information")]
        public string sceneName;
    }
}
