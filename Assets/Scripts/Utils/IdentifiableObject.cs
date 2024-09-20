using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
    public abstract class IdentifiableObject : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected string id;

        /// <summary>
        /// Make sure to call base.OnValidate() in override if you need OnValidate.
        /// </summary>
        protected virtual void OnValidate()
        {
            #if UNITY_EDITOR
            if (string.IsNullOrEmpty(id) && !PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                GenerateGuid();
            }
            #endif
        }

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}