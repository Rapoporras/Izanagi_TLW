using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
    public abstract class IdentifiableObject : MonoBehaviour
    {
        [ReadOnly] public string id;

        /// <summary>
        /// Make sure to call base.OnValidate() in override if you need OnValidate.
        /// </summary>
        protected virtual void OnValidate()
        {
            #if UNITY_EDITOR
            if (!PrefabUtility.IsPartOfPrefabAsset(gameObject) && !Application.isPlaying)
            {
                // creating a new object
                if (string.IsNullOrEmpty(id))
                {
                    GenerateGuid();
                }
                // duplicating object in scene
                else
                {
                    
                    var identifiableObjects = FindObjectsOfType<IdentifiableObject>();
                    var ids = identifiableObjects
                        .Where(obj => obj != this)
                        .Select(obj => obj.id)
                        .ToList();
                    
                    // avoid having same id when duplicating object
                    while (ids.Contains(id))
                    {
                        GenerateGuid();
                    }
                }
            }
            #endif
        }

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }
        
        [ContextMenu("Clear object id")]
        private void ClearGuid()
        {
            id = string.Empty;
        }
    }
}