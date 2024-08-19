#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CustomMenus
{
    
    public class EnemiesCustomMenu
    {
        private static string basePath = "Assets/Prefabs/Enemies/";

        private static string basicEnemyPath = basePath + "BasicEnemy.prefab";
        private static string chaserPath = basePath + "Chaser.prefab";

        [MenuItem("GameObject/IzanagiTLW/Enemies/Chaser", false, 10)]
        private static void CreateChaser()
        {
            InstantiatePrefabAtPath(chaserPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Enemies/Basic", false, 10)]
        private static void CreateBasicEnemy()
        {
            InstantiatePrefabAtPath(basicEnemyPath);
        }

        private static void InstantiatePrefabAtPath(string path)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (!prefab)
            {
                Debug.LogError($"Prefab not found! Route: {path}");
                return;
            }

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (!instance)
            {
                Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
                Selection.activeObject = instance;
            }
        }
    }
}
#endif
