using System.Collections.Generic;
using GlobalVariables;
using PlayerController.Abilities;
using PlayerController.Data;
using SaveSystem;
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

        [Header("Player Attack")]
        [SerializeField] private FloatReference _attackMultiplier;

        [Header("Player Data")]
        [SerializeField] private PlayerAbilitiesData _playerAbilitiesData;

        [Header("Temporal Data")]
        [SerializeField] private TemporalDataSO _temporalData;

        private static GameInitializer _instance;
        private bool _variablesLoaded;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void LoadData(GameData data)
        {
            if (_variablesLoaded) return;
            
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

            _temporalData.Clear();
            
            _variablesLoaded = true;
        }

        public void SaveData(ref GameData data)
        {
            data.variables.maxHealth = _playerMaxHealth;
            data.variables.maxPotions = _playerMaxPotions;
            
            data.variables.attackItemAmount = _attackItemAmount;
            data.variables.healthItemAmount = _healthItemAmount;

            data.variables.attackMultiplier = Mathf.Round(_attackMultiplier * 100f) / 100f;

            data.abilitiesUnlocked = _playerAbilitiesData.GetAbilitiesList();
        }
    }
}