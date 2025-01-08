using System.Collections.Generic;
using GlobalVariables;
using PlayerController.Abilities;
using PlayerController.Data;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;

namespace Managers
{
    public class GameInitializer : MonoBehaviour, IDataPersistence
    {
        [Header("Health Variables")]
        [SerializeField] private IntReference _playerMaxHealth;
        [SerializeField] private IntReference _playerCurrentHealth;
        [Space(5)]
        [SerializeField] private IntReference _playerMaxPotions;
        [SerializeField] private IntReference _playerPotionsAvailable;

        [Header("Items Variables")]
        [SerializeField] private IntReference _attackItemAmount;
        [SerializeField] private IntReference _healthItemAmount;

        [Header("Manna Variables")]
        [SerializeField] private IntReference _currentManna;

        [Header("Player")]
        [SerializeField] private FloatReference _attackMultiplier;
        [Space(5)]
        [SerializeField] private PlayerAbilitiesData _playerAbilitiesData;
        [Space(5)]
        [SerializeField] private PlayerPathSO _playerPath;

        [Header("Temporal Data")]
        [SerializeField] private TemporalDataSO _temporalData;

        private static GameInitializer _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void LoadData(GameData data)
        {
            _playerMaxHealth.Value = data.variables.maxHealth;
            _playerCurrentHealth.Value = data.variables.maxHealth;
            
            _playerMaxPotions.Value = data.variables.maxPotions;
            _playerPotionsAvailable.Value = data.variables.maxPotions;
            
            _attackItemAmount.Value = data.variables.attackItemAmount;
            _healthItemAmount.Value = data.variables.healthItemAmount;

            _currentManna.Value = 0;

            _attackMultiplier.Value = data.variables.attackMultiplier;

            List<int> abilities = _playerAbilitiesData.GetAbilitiesList();
            foreach (var ability in abilities)
            {
                _playerAbilitiesData.SetAbilityStatus((AbilityType) ability, data.abilitiesUnlocked.Contains(ability));
            }

            _playerPath.Clear();
            _temporalData.Clear();
        }

        // don't need to save any data
        public void SaveData(ref GameData data) { }
    }
}