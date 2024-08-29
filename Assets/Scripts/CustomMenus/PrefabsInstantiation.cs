#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CustomMenus
{
    
    public static class PrefabsInstantiation
    {
        #region BASE
        private const string BasePath = "Assets/Prefabs/";
        
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
        #endregion
        
        #region PLAYER
        private const string PlayerPath = BasePath + "Player/Player.prefab";

        [MenuItem("GameObject/IzanagiTLW/Player", false, 0)]
        private static void CreatePlayer()
        {
            InstantiatePrefabAtPath(PlayerPath);
        }
        #endregion
        
        #region ITEMS
        private const string HealthItemPath = BasePath + "Items/Health Item.prefab";
        private const string AttackItemPath = BasePath + "Items/Attack Item.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/Items/Health", false, 0)]
        private static void CreateHealthItem()
        {
            InstantiatePrefabAtPath(HealthItemPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Items/Attack", false, 0)]
        private static void CreateAttackItem()
        {
            InstantiatePrefabAtPath(AttackItemPath);
        }
        #endregion
        
        #region ENEMIES
        private const string BasicEnemyPath = BasePath + "Enemies/BasicEnemy.prefab";
        private const string ChaserPath = BasePath + "Enemies/Chaser.prefab";

        [MenuItem("GameObject/IzanagiTLW/Enemies/Chaser", false, 0)]
        private static void CreateChaser()
        {
            InstantiatePrefabAtPath(ChaserPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Enemies/Basic", false, 0)]
        private static void CreateBasicEnemy()
        {
            InstantiatePrefabAtPath(BasicEnemyPath);
        }
        #endregion
    }
}
#endif
