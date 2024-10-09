using System.Collections.Generic;
using System.Linq;
using SceneLoaderSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveSystem
{
    public class TemporalDataManager : MonoBehaviour
    {
        [Header("Data")]
        public TemporalDataSO temporalData;
        
        private List<ITemporalDataPersistence> _temporalDataObjects = new List<ITemporalDataPersistence>();
        
        public static TemporalDataManager Instance { get; private set; }

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
        }
        
        public void OnSceneLoaded()
        {
            _temporalDataObjects = FindAllTemporalDataObjects();
            LoadGame();
        }

        private List<ITemporalDataPersistence> FindAllTemporalDataObjects()
        {
            IEnumerable<ITemporalDataPersistence> dataPersistenceObjects = 
                FindObjectsOfType<MonoBehaviour>(true).OfType<ITemporalDataPersistence>();

            return new List<ITemporalDataPersistence>(dataPersistenceObjects);
        }

        public void LoadGame()
        {
            if (!temporalData)
                return;
            
            foreach (var temporalDataObject in _temporalDataObjects)
            {
                temporalDataObject.LoadTemporalData(temporalData);
            }
        }
        
        public void SaveGame()
        {
            if (!temporalData)
                return;

            foreach (var temporalDataObject in _temporalDataObjects)
            {
                temporalDataObject.SaveTemporalData(temporalData);
            }
        }

        public void SaveGameWithRequest(LoadSceneRequest request)
        {
            if (request.requestFromDeath) return;
            
            SaveGame();
        }
    }
}