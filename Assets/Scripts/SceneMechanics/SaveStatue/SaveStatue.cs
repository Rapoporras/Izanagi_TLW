using System.Collections.Generic;
using DialogueSystem;
using GlobalVariables;
using InteractionSystem;
using PlayerController.Data;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;
using Utils;
using Utils.CustomLogs;

namespace SceneMechanics.SaveStatue
{
    public class SaveStatue : IdentifiableObject, IInteractable, IDataPersistence
    {
        [Header("Variables")]
        [SerializeField] private IntReference _playerMaxHealth;
        [SerializeField] private IntReference _playerCurrentHealth;
        [Space(5)]
        [SerializeField] private IntReference _playerMaxPotions;
        [SerializeField] private IntReference _playerPotionsAvailable;
        
        [Header("Symbols")]
        [SerializeField] private List<AbilitySymbol> _abilitySymbols;
        
        [Header("Scene Data")]
        [SerializeField] private SceneSO _currentScene;
        [Space(5)]
        [SerializeField] private LevelEntrance _saveStatueEntrance;
        
        [Header("Player Data")]
        [SerializeField] private PlayerAbilitiesData _abilitiesData;
        [SerializeField] private PlayerPathSO _playerPath;
        
        [Header("UI")]
        [SerializeField] private GameObject _interactUIText;

        private AudioSource _audioSource;

        private List<BaseEnemy> _sceneEnemies = new List<BaseEnemy>();

        public LevelEntrance Entrance => _saveStatueEntrance;

        private void Awake()
        {
            _sceneEnemies = FindAllEnemiesInScene();

            _audioSource = GetComponent<AudioSource>();
        }

        public void Interact(Interactor interactor)
        {
            _playerCurrentHealth.Value = _playerMaxHealth;
            _playerPotionsAvailable.Value = _playerMaxPotions;

            _playerPath.lastSavePoint = _saveStatueEntrance.entrance;
            
            // DataPersistenceManager.Instance.gameData.lastSaveScene = _currentScene.sceneName;
            DataPersistenceManager.Instance.gameData.lastSaveInfo.sceneName = _currentScene.sceneName;
            DataPersistenceManager.Instance.gameData.lastSaveInfo.statueId = id;
            DataPersistenceManager.Instance.SaveGame();

            TemporalDataManager.Instance.temporalData.Clear();
            RespawnEnemiesInScene();

            DialogueManager.Instance.SaveVariables();

            ActivateSymbols();
            ControllerVibration.Instance.TriggerProgressiveVibration(0.5f, 0.5f);
            _audioSource.Play();
            LogManager.Log("Game saved . . .", FeatureType.SaveSystem);
        }

        public void ShowInteractionUI(bool showUI)
        {
            _interactUIText.SetActive(showUI);
        }

        private List<BaseEnemy> FindAllEnemiesInScene()
        {
            IEnumerable<BaseEnemy> enemies = FindObjectsOfType<BaseEnemy>();

            return new List<BaseEnemy>(enemies);
        }

        private void RespawnEnemiesInScene()
        {
            foreach (var enemy in _sceneEnemies)
            {
                enemy.gameObject.SetActive(true);
            }
        }
        
        public void ActivateSymbols()
        {
            LogManager.Log("Activate Symbols", FeatureType.SaveSystem);
            foreach (var symbol in _abilitySymbols)
            {
                if (_abilitiesData.IsAbilityUnlock(symbol.AbilityType))
                    symbol.TurnOn();
                else
                    symbol.TurnOff();
            }
        }

        public void LoadData(GameData data)
        {
            if (_playerPath.lastSavePoint != null
                &&_playerPath.lastSavePoint == _saveStatueEntrance.entrance)
            {
                ActivateSymbols();
            }
        }

        public void SaveData(ref GameData data) { }
    }
}