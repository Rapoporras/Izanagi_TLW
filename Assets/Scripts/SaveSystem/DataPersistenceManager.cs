﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        
        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjects = new List<IDataPersistence>();
        private FileDataHandler _dataHandler;
        
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

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
            SaveGame();
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = 
                FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public void NewGame()
        {
            _gameData = new GameData();
            SaveGame();
        }

        public void LoadGame()
        {
            // load any saved data from a file using the data handler
            _gameData = _dataHandler.Load();
            
            // if no data can be loaded, don't continue
            if (_gameData == null)
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be loaded");
                return;
            }
            
            // push the loaded data to all other scripts that need it
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            if (_gameData == null)
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
                return;
            }
            
            // pass the data to other scripts so they can update it
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(ref _gameData);
            }
            
            // save that data to a file using the data handler
            _dataHandler.Save(_gameData);
        }

        public bool HasGameData()
        {
            return _dataHandler.ExistsFile();
        }
    }
}