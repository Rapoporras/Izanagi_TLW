using System.Collections.Generic;
using GlobalVariables;
using InteractionSystem;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;

namespace SceneMechanics
{
    public class SavePoint : MonoBehaviour, IInteractable
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

        private List<BaseEnemy> _sceneEnemies = new List<BaseEnemy>();

        private void Awake()
        {
            _sceneEnemies = FindAllEnemiesInScene();
        }

        public void Interact(Interactor interactor)
        {
            _playerCurrentHealth.Value = _playerMaxHealth;
            _playerPotionsAvailable.Value = _playerMaxPotions;
            
            TemporalDataManager.Instance.temporalData.EnemiesStatus.Clear();
            
            DataPersistenceManager.Instance.gameData.lastSaveScene = _currentScene.sceneName;
            DataPersistenceManager.Instance.SaveGame();

            RespawnEnemiesInScene();
            
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
    }
}