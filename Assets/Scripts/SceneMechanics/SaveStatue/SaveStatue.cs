using System.Collections.Generic;
using GlobalVariables;
using InteractionSystem;
using PlayerController;
using PlayerController.Data;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;

namespace SceneMechanics.SaveStatue
{
    public class SaveStatue : MonoBehaviour, IInteractable, IDataPersistence
    {
        [Header("Health Variables")]
        [SerializeField] private IntReference _playerMaxHealth;
        [SerializeField] private IntReference _playerCurrentHealth;
        [Space(5)]
        [SerializeField] private IntReference _playerMaxPotions;
        [SerializeField] private IntReference _playerPotionsAvailable;
        
        [Header("Scene Data")]
        [SerializeField] private SceneSO _currentScene;
        
        [Header("UI")]
        [SerializeField] private GameObject _interactUIText;

        [Header("Abilities Data")]
        [SerializeField] private PlayerAbilitiesData _abilitiesData;

        [Header("Symbols")]
        [SerializeField] private List<AbilitySymbol> _abilitySymbols;

        private List<BaseEnemy> _sceneEnemies = new List<BaseEnemy>();

        private void Awake()
        {
            _sceneEnemies = FindAllEnemiesInScene();
        }

        public void Interact(Interactor interactor)
        {
            _playerCurrentHealth.Value = _playerMaxHealth;
            _playerPotionsAvailable.Value = _playerMaxPotions;
            
            DataPersistenceManager.Instance.gameData.lastSaveScene = _currentScene.sceneName;
            DataPersistenceManager.Instance.SaveGame();

            TemporalDataManager.Instance.temporalData.EnemiesStatus.Clear();
            RespawnEnemiesInScene();

            ActivateSymbols();
            
            Debug.Log("Game saved . . .");
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
        
        private void ActivateSymbols()
        {
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
            if (data.lastSaveScene == _currentScene.sceneName)
            {
                ActivateSymbols();
            }
        }

        public void SaveData(ref GameData data) { }
    }
}