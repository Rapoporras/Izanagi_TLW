#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Path = Ink.Runtime.Path;

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
            if (instance)
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

        private const string MiniKappaSpawnerPath = BasePath + "Enemies/Kappas/MiniKappas Spawner.prefab";

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
        
        [MenuItem("GameObject/IzanagiTLW/Enemies/Kappas/Mini Kappas Spawner", false, 0)]
        private static void CreateMiniKappasSpawner()
        {
            InstantiatePrefabAtPath(MiniKappaSpawnerPath);
        }
        #endregion
        
        #region SCENE ELEMENTS
        private const string BreakableWallPath = BasePath + "Scene Elements/Breakable Wall.prefab";
        private const string BrittleSoilPath = BasePath + "Scene Elements/Brittle Soil.prefab";
        private const string StalactitePath = BasePath + "Scene Elements/Stalactite.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/Scene Elements/Breakable Wall", false, 0)]
        private static void CreateBreakableWall()
        {
            InstantiatePrefabAtPath(BreakableWallPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Scene Elements/Brittle Soil", false, 0)]
        private static void CreateBrittleSoil()
        {
            InstantiatePrefabAtPath(BrittleSoilPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Scene Elements/Stalactite", false, 0)]
        private static void CreateStalactite()
        {
            InstantiatePrefabAtPath(StalactitePath);
        }
        #endregion
        
        #region UI
        private const string DialogueCanvasPath = BasePath + "UI/Dialogue Canvas.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/UI/Dialogue Canvas", false, 0)]
        private static void CreateDialogueCanvas()
        {
            InstantiatePrefabAtPath(DialogueCanvasPath);
        }
        #endregion
        
        #region MANAGERS
        private const string DeathManagerPath = BasePath + "Managers/Death Manager.prefab";
        private const string InputManagerPath = BasePath + "Managers/Input Manager.prefab";
        private const string ItemManagerPath = BasePath + "Managers/Item Manager.prefab";
        private const string SaveManagerPath = BasePath + "Managers/Save Manager.prefab";
        private const string VariablesInitializerPath = BasePath + "Managers/Variables Initializer.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Death Manager", false, 0)]
        private static void CreateDeathManager()
        {
            InstantiatePrefabAtPath(DeathManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Input Manager", false, 0)]
        private static void CreateInputManager()
        {
            InstantiatePrefabAtPath(InputManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Item Manager", false, 0)]
        private static void CreateItemManager()
        {
            InstantiatePrefabAtPath(ItemManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Save Manager", false, 0)]
        private static void CreateSaveManager()
        {
            InstantiatePrefabAtPath(SaveManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Variables Initializer", false, 0)]
        private static void CreateVariablesInitializer()
        {
            InstantiatePrefabAtPath(VariablesInitializerPath);
        }
        #endregion
    }
}
#endif
