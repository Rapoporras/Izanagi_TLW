using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.CustomLogs;

namespace SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        
        private List<IDataPersistence> _dataPersistenceObjects = new List<IDataPersistence>();
        private FileDataHandler _dataHandler;
        
        [HideInInspector] public GameData gameData;
        public static DataPersistenceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        }

        public void OnSceneLoaded() // called from listener
        {
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }
        
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = 
                FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public void NewGame(string firstSceneName)
        {
            gameData = new GameData();
            // gameData.lastSaveScene = firstSceneName;
            gameData.lastSaveInfo.sceneName = firstSceneName;
            _dataHandler.Save(gameData);
        }

        public void LoadGame()
        {
            // load any saved data from a file using the data handler
            gameData = _dataHandler.Load();
            
            // if no data can be loaded, don't continue
            if (gameData == null)
            {
                LogManager.LogWarning("No data was found. A New Game needs to be started before data can be loaded",
                    FeatureType.SaveSystem);
                return;
            }
            
            // push the loaded data to all other scripts that need it
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            if (gameData == null)
            {
                LogManager.LogWarning("No data was found. A New Game needs to be started before data can be saved",
                    FeatureType.SaveSystem);
                return;
            }
            
            // pass the data to other scripts so they can update it
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(ref gameData);
            }
            
            // save that data to a file using the data handler
            _dataHandler.Save(gameData);
        }

        public bool HasGameData()
        {
            return _dataHandler.ExistsFile();
        }

        public GameData GetGameData()
        {
            return _dataHandler.Load();
        }
    }
}