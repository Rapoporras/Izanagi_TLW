#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Utils.CustomLogs;

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
        private const string SaveStatuePath = BasePath + "Scene Elements/Save Statue.prefab";
        private const string NPCBasePath = BasePath + "Scene Elements/NPC Base.prefab";
        
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
        
        [MenuItem("GameObject/IzanagiTLW/Scene Elements/Save Statue", false, 0)]
        private static void CreateSaveStatue()
        {
            InstantiatePrefabAtPath(SaveStatuePath);
            LogManager.LogWarning("New save statue instantiated, set LevelEntrance (in child object Entrance)" +
                             " and CurrentScene manually.", FeatureType.SaveSystem);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Scene Elements/NPC Base", false, 0)]
        private static void CreateNPCBase()
        {
            InstantiatePrefabAtPath(NPCBasePath);
        }
        #endregion
        
        #region UI
        private const string DialogueCanvasPath = BasePath + "UI/Dialogue Canvas.prefab";
        private const string LoadingScreenCanvasPath = BasePath + "UI/Loading Screen Canvas.prefab";
        private const string PlayerCanvasPath = BasePath + "UI/Player Canvas.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/UI/Dialogue Canvas", false, 0)]
        private static void CreateDialogueCanvas()
        {
            InstantiatePrefabAtPath(DialogueCanvasPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/UI/Loading Screen Canvas", false, 0)]
        private static void CreateLoadingScreenCanvas()
        {
            InstantiatePrefabAtPath(LoadingScreenCanvasPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/UI/Player Canvas", false, 0)]
        private static void CreatePlayerCanvas()
        {
            InstantiatePrefabAtPath(PlayerCanvasPath);
        }
        #endregion
        
        #region MANAGERS
        private const string DeathManagerPath = BasePath + "Managers/Gameplay/Death Manager.prefab";
        private const string ItemManagerPath = BasePath + "Managers/Gameplay/Item Manager.prefab";
        private const string SaveManagerPath = BasePath + "Managers/Save Manager.prefab";
        private const string GameInitializerPath = BasePath + "Managers/Game Initializer.prefab";
        private const string SceneInitializerPath = BasePath + "Managers/Scene/Scene Initializer.prefab";
        private const string SceneLoaderManagerPath = BasePath + "Managers/Scene/Scene Loader Manager.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Gameplay/Death Manager", false, 0)]
        private static void CreateDeathManager()
        {
            InstantiatePrefabAtPath(DeathManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Gameplay/Item Manager", false, 0)]
        private static void CreateItemManager()
        {
            InstantiatePrefabAtPath(ItemManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Save Manager", false, 0)]
        private static void CreateSaveManager()
        {
            InstantiatePrefabAtPath(SaveManagerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Game Initializer", false, 0)]
        private static void CreateGameInitializer()
        {
            InstantiatePrefabAtPath(GameInitializerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Scene/Scene Initializer", false, 0)]
        private static void CreateSceneInitializer()
        {
            InstantiatePrefabAtPath(SceneInitializerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Managers/Scene/Scene Loader Manager", false, 0)]
        private static void CreateSceneLoaderManager()
        {
            InstantiatePrefabAtPath(SceneLoaderManagerPath);
        }
        #endregion
        
        #region CAMERA SYSTEM
        private const string CameraPath = BasePath + "CameraSystem/Cameras.prefab";
        private const string CameraControlTriggerPath = BasePath + "CameraSystem/Camera Control Trigger.prefab";
        private const string LightPointPath = BasePath + "CameraSystem/LightPoint/LightPoint.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/CameraSystem/Cameras", false, 0)]
        private static void CreateCamera()
        {
            InstantiatePrefabAtPath(CameraPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/CameraSystem/Camera Control Trigger", false, 0)]
        private static void CreateCameraControlTrigger()
        {
            InstantiatePrefabAtPath(CameraControlTriggerPath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/CameraSystem/LightPoint/LightPoint", false, 0)]
        private static void CreateLightPoint()
        {
            InstantiatePrefabAtPath(LightPointPath);
        }
        #endregion
        
        #region LEVEL
        private const string LevelEntrancePath = BasePath + "Level/Level Entrance.prefab";
        private const string LevelExitPath = BasePath + "Level/Level Exit.prefab";
        
        [MenuItem("GameObject/IzanagiTLW/Level/Entrance", false, 0)]
        private static void CreateLevelEntrance()
        {
            InstantiatePrefabAtPath(LevelEntrancePath);
        }
        
        [MenuItem("GameObject/IzanagiTLW/Level/Exit", false, 0)]
        private static void CreateLevelExit()
        {
            InstantiatePrefabAtPath(LevelExitPath);
        }
        #endregion
    }
}
#endif
